using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Prescriptions;
using Api.Endpoints.Models.Appointments;
using Api.Endpoints.Models.Prescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers.Prescriptions;

[ApiController]
[Route("api/prescriptions")]
[Authorize]
public class PrescriptionsController(AppDbContext db) : ControllerBase
{
    // POST /api/prescriptions
    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<ActionResult<PrescriptionResponse>> Create([FromBody] CreatePrescriptionRequest body, CancellationToken ct)
    {
        if (body.AppointmentId == Guid.Empty) return BadRequest("AppointmentId required.");
        if (body.PatientUserId == Guid.Empty) return BadRequest("PatientUserId required.");
        if (body.Items is null || body.Items.Count == 0) return BadRequest("At least one item required.");

        var ap = await db.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == body.AppointmentId, ct);
        if (ap is null) return NotFound("Appointment not found.");
        if (ap.PatientUserId != body.PatientUserId) return BadRequest("Patient/Appointment mismatch.");
        if (ap.Status == AppointmentStatus.Cancelled) return BadRequest("Cancelled appointment.");

        var (me, _) = GetUserIdAndRole();
        if (!me.HasValue) return Forbid();

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
            pres.Items.Add(new PrescriptionItem
            {
                DrugName     = it.DrugName.Trim(),
                GenericName  = string.IsNullOrWhiteSpace(it.GenericName) ? null : it.GenericName.Trim(),
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

        return Ok(Map(pres));
    }

    // GET /api/prescriptions/{id}
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

        return Ok(Map(pres));
    }

    // GET /api/prescriptions/by-patient/{patientUserId}
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("by-patient/{patientUserId:guid}")]
    public async Task<ActionResult<List<PrescriptionResponse>>> ListByPatient(Guid patientUserId, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();
        if (role == "Patient" && uid != patientUserId) return Forbid();

        var list = await db.Prescriptions
            .Include(p => p.Items)
            .AsNoTracking()
            .Where(p => p.PatientUserId == patientUserId)
            .OrderByDescending(p => p.IssuedAtUtc)
            .ToListAsync(ct);

        return Ok(list.Select(Map).ToList());
    }

    // PUT /api/prescriptions/{id}/cancel
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

    // PUT /api/prescriptions/{id}/dispense
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

    private PrescriptionResponse Map(Prescription p) => new()
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
        Items = p.Items.Select(i => new PrescriptionItemDto
        {
            Id           = i.Id,
            DrugName     = i.DrugName,
            GenericName  = i.GenericName,
            Dosage       = i.Dosage,
            Frequency    = i.Frequency,
            Duration     = i.Duration,
            Instructions = i.Instructions,
            IsPRN        = i.IsPRN,
            RefillCount  = i.RefillCount
        }).ToList()
    };

    private static string GenerateErxCode() =>
        $"ERX-{Convert.ToHexString(Guid.NewGuid().ToByteArray())[..10]}";

    private (Guid? userId, string role) GetUserIdAndRole()
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ??
                   User.FindFirstValue("role") ??
                   User.FindFirstValue("roles") ?? "";
        var   sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        Guid? uid = null;
        if (Guid.TryParse(sub, out var parsed)) uid = parsed;
        return (uid, role);
    }
}