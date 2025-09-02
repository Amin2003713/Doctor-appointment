#nullable enable
using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Prescriptions;
using Api.Endpoints.Models.Appointments;
using Api.Endpoints.Models.Prescriptions;
using Api.Endpoints.Models.Drugs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers.Prescriptions;

[ApiController]
[Route("api/prescriptions")]
[Authorize]
public class PrescriptionsController(AppDbContext db) : ControllerBase
{
    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<ActionResult<PrescriptionResponse>> Create([FromBody] CreatePrescriptionRequest body, CancellationToken ct)
    {
        if (body.AppointmentId == Guid.Empty) return BadRequest("AppointmentId required.");
        if (body.PatientUserId == 0) return BadRequest("PatientUserId required.");
        if (body.Items is null || body.Items.Count == 0) return BadRequest("At least one item required.");

        var ap = await db.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == body.AppointmentId, ct);
        if (ap is null) return NotFound("Appointment not found.");
        if (ap.PatientUserId != body.PatientUserId) return BadRequest("Patient/Appointment mismatch.");
        if (ap.Status == AppointmentStatus.Cancelled) return BadRequest("Cancelled appointment.");

        var (me, _) = GetUserIdAndRole();
        if (!me.HasValue) return Forbid();

        // Preload catalog rows once
        var drugIds = body.Items.Where(i => i.DrugId.HasValue).Select(i => i.DrugId!.Value).Distinct().ToList();
        var drugMap = drugIds.Count == 0
            ? new Dictionary<Guid, Drug>()
            : await db.Drugs.AsNoTracking().Where(d => drugIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, ct);

        // Per-item validation
        foreach (var it in body.Items)
        {
            if (string.IsNullOrWhiteSpace(it.Dosage) || string.IsNullOrWhiteSpace(it.Frequency) || string.IsNullOrWhiteSpace(it.Duration))
                return BadRequest("Dosage, Frequency and Duration are required for each item.");

            if (it.DrugId.HasValue && !drugMap.ContainsKey(it.DrugId.Value))
                return BadRequest($"Drug not found: {it.DrugId}");
            if (!it.DrugId.HasValue && string.IsNullOrWhiteSpace(it.DrugName))
                return BadRequest("Either DrugId or DrugName must be provided for each item.");
        }

        var issueMethod = (IssueMethod)body.IssueMethod;

        var pres = new Prescription
        {
            AppointmentId      = ap.Id,
            PatientUserId      = body.PatientUserId,
            PrescribedByUserId = me.Value,
            Notes              = string.IsNullOrWhiteSpace(body.Notes) ? null : body.Notes.Trim(),
            Status             = PrescriptionStatus.Issued,
            IssueMethod        = issueMethod,
            ErxCode            = issueMethod == IssueMethod.Electronic ? GenerateErxCode() : null
        };

        foreach (var it in body.Items)
        {
            Drug? d = it.DrugId.HasValue ? drugMap[it.DrugId.Value] : null;
            var finalDrugName = !string.IsNullOrWhiteSpace(it.DrugName)
                ? it.DrugName!.Trim()
                : (d is not null ? d.BrandName : throw new InvalidOperationException("DrugName resolution failed."));

            pres.Items.Add(new PrescriptionItem
            {
                DrugId       = it.DrugId,
                DrugName     = finalDrugName,
                Dosage       = it.Dosage.Trim(),
                Frequency    = it.Frequency.Trim(),
                Duration     = it.Duration.Trim(),
                Instructions = string.IsNullOrWhiteSpace(it.Instructions) ? null : it.Instructions.Trim(),
                IsPRN        = it.IsPRN,
                RefillCount  = it.RefillCount
            });
        }

        db.Prescriptions.Add(pres);
        await db.SaveChangesAsync(ct);

        return Ok(Map(pres, drugMap)); // includes GenericName from catalog
    }

    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PrescriptionResponse>> GetById(Guid id, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();

        var pres = await db.Prescriptions
            .Include(p => p.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        if (pres is null) return NotFound();
        if (role == "Patient" && pres.PatientUserId != uid) return Forbid();

        var drugIds = pres.Items.Where(i => i.DrugId.HasValue).Select(i => i.DrugId!.Value).Distinct().ToList();
        var drugMap = drugIds.Count == 0
            ? new Dictionary<Guid, Drug>()
            : await db.Drugs.AsNoTracking().Where(d => drugIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, ct);

        return Ok(Map(pres, drugMap));
    }

    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("by-patient/{patientUserId:long}")]
    public async Task<ActionResult<List<PrescriptionResponse>>> ListByPatient(long patientUserId, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();
        if (role == "Patient" && uid != patientUserId) return Forbid();

        var list = await db.Prescriptions
            .Include(p => p.Items)
            .AsNoTracking()
            .Where(p => p.PatientUserId == patientUserId)
            .OrderByDescending(p => p.IssuedAtUtc)
            .ToListAsync(ct);

        var drugIds = list.SelectMany(p => p.Items).Where(i => i.DrugId.HasValue).Select(i => i.DrugId!.Value).Distinct().ToList();
        var drugMap = drugIds.Count == 0
            ? new Dictionary<Guid, Drug>()
            : await db.Drugs.AsNoTracking().Where(d => drugIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, ct);

        return Ok(list.Select(p => Map(p, drugMap)).ToList());
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var pres = await db.Prescriptions.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pres is null) return NotFound();
        if (pres.Status == PrescriptionStatus.Cancelled) return NoContent();
        if (pres.Status == PrescriptionStatus.Dispensed) return BadRequest("Dispensed prescriptions cannot be cancelled.");

        pres.Status = PrescriptionStatus.Cancelled;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("{id:guid}/dispense")]
    public async Task<IActionResult> MarkDispensed(Guid id, CancellationToken ct)
    {
        var pres = await db.Prescriptions.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pres is null) return NotFound();
        if (pres.Status == PrescriptionStatus.Cancelled) return BadRequest("Cancelled prescription.");

        pres.Status = PrescriptionStatus.Dispensed;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // ------------ helpers ------------
    private PrescriptionResponse Map(Prescription p, IReadOnlyDictionary<Guid, Drug> drugMap)
        => new()
        {
            Id                 = p.Id,
            AppointmentId      = p.AppointmentId,
            PatientUserId      = p.PatientUserId,
            PrescribedByUserId = p.PrescribedByUserId,
            IssuedAtUtc        = p.IssuedAtUtc,
            Status             = (int)p.Status,
            IssueMethod        = (int)p.IssueMethod,
            ErxCode            = p.ErxCode,
            Notes              = p.Notes,
            Items = p.Items.Select(i =>
            {
                drugMap.TryGetValue(i.DrugId ?? Guid.Empty, out var d);
                return new PrescriptionItemDto
                {
                    Id           = i.Id,
                    DrugId       = i.DrugId,
                    DrugName     = i.DrugName,
                    GenericName  = d?.GenericName, // derived from catalog
                    Dosage       = i.Dosage,
                    Frequency    = i.Frequency,
                    Duration     = i.Duration,
                    Instructions = i.Instructions,
                    IsPRN        = i.IsPRN,
                    RefillCount  = i.RefillCount
                };
            }).ToList()
        };

    private static string GenerateErxCode()
        => $"ERX-{Convert.ToHexString(Guid.NewGuid().ToByteArray())[..10]}";

    private (long? userId, string role) GetUserIdAndRole()
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ??
                   User.FindFirstValue("role") ??
                   User.FindFirstValue("roles") ?? "";
        var sub  = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        long? uid = null;
        if (long.TryParse(sub, out var parsed)) uid = parsed;
        return (uid, role);
    }
}
