#nullable enable
using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Prescriptions;
using Api.Endpoints.Models.Appointments;
using Api.Endpoints.Models.Prescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// ⬅ Prescription, PrescriptionItem, enums

namespace Api.Endpoints.Controllers.Prescriptions;

#region MedicalRecordsController
[ApiController]
[Route("api/medical-records")]
[Authorize]
public class MedicalRecordsController(AppDbContext db) : ControllerBase
{
    // GET /api/medical-records/{patientUserId}/ehr
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("{patientUserId:long}/ehr")]
    public async Task<ActionResult<PatientEhrResponse>> GetEhr(long patientUserId, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();
        if (role == "Patient" && uid != patientUserId) return Forbid();

        // Try to load profile basics from latest appointment (fallback strategy)
        var lastAppt = await db.Appointments.AsNoTracking()
            .Where(a => a.PatientUserId == patientUserId)
            .OrderByDescending(a => a.Date)
            .FirstOrDefaultAsync(ct);

        var fullName = lastAppt?.PatientFullName ?? "Patient";
        var phone    = lastAppt?.PatientPhone ?? "-";
        string? address = null; // load from users table if you have it

        var appts = await db.Appointments.AsNoTracking()
            .Where(a => a.PatientUserId == patientUserId)
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.Start)
            .Select(a => new AppointmentSummary
            {
                Id    = a.Id,
                Date  = a.Date,
                Start = a.Start.ToString("HH:mm"),
                End   = a.End.ToString("HH:mm"),
                Status = a.Status,
                Notes  = a.Notes
            })
            .ToListAsync(ct);

        var apptIds = appts.Select(a => a.Id).ToList();

        var records = await db.MedicalRecords.AsNoTracking()
            .Where(r => apptIds.Contains(r.AppointmentId))
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new MedicalRecordResponse
            {
                Id             = r.Id,
                AppointmentId  = r.AppointmentId,
                Notes          = r.Notes,
                Diagnosis      = r.Diagnosis,
                PrescriptionText = r.PrescriptionText,
                AttachmentsUrl = r.AttachmentsUrl,
                CreatedAt      = r.CreatedAtUtc
            })
            .ToListAsync(ct);

        var prescriptions = await db.Prescriptions.AsNoTracking()
            .Include(p => p.Items)
            .Where(p => p.PatientUserId == patientUserId)
            .OrderByDescending(p => p.IssuedAtUtc)
            .Select(p => new PrescriptionResponse
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
            })
            .ToListAsync(ct);

        var res = new PatientEhrResponse
        {
            PatientUserId = patientUserId,
            FullName      = fullName,
            PhoneNumber   = phone,
            Address       = address,
            Appointments  = appts,
            MedicalRecords = records,
            Prescriptions = prescriptions
        };

        return Ok(res);
    }

    // POST /api/medical-records
    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPost]
    public async Task<ActionResult<MedicalRecordResponse>> Upsert([FromBody] UpsertMedicalRecordRequest body, CancellationToken ct)
    {
        if (body.AppointmentId == Guid.Empty) return BadRequest("AppointmentId required.");
        if (string.IsNullOrWhiteSpace(body.Notes)) return BadRequest("Notes required.");

        var ap = await db.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == body.AppointmentId, ct);
        if (ap is null) return NotFound("Appointment not found.");
        if (ap.Status == AppointmentStatus.Cancelled) return BadRequest("Cancelled appointment.");

        var rec = new MedicalRecord
        {
            Id            = Guid.NewGuid(),
            AppointmentId = ap.Id,
            Notes         = body.Notes.Trim(),
            Diagnosis     = string.IsNullOrWhiteSpace(body.Diagnosis) ? null : body.Diagnosis.Trim(),
            PrescriptionText = string.IsNullOrWhiteSpace(body.PrescriptionText) ? null : body.PrescriptionText.Trim(),
            AttachmentsUrl   = string.IsNullOrWhiteSpace(body.AttachmentsUrl) ? null : body.AttachmentsUrl.Trim(),
            CreatedAtUtc  = DateTime.UtcNow
        };

        db.MedicalRecords.Add(rec);
        await db.SaveChangesAsync(ct);

        return Ok(new MedicalRecordResponse
        {
            Id             = rec.Id,
            AppointmentId  = rec.AppointmentId,
            Notes          = rec.Notes,
            Diagnosis      = rec.Diagnosis,
            PrescriptionText = rec.PrescriptionText,
            AttachmentsUrl = rec.AttachmentsUrl,
            CreatedAt      = rec.CreatedAtUtc
        });
    }

    // GET /api/medical-records/by-appointment/{appointmentId}
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("by-appointment/{appointmentId:guid}")]
    public async Task<ActionResult<List<MedicalRecordResponse>>> ByAppointment(Guid appointmentId, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();

        var ap = await db.Appointments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == appointmentId, ct);
        if (ap is null) return NotFound();

        if (role == "Patient" && ap.PatientUserId != uid) return Forbid();

        var list = await db.MedicalRecords.AsNoTracking()
            .Where(r => r.AppointmentId == appointmentId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new MedicalRecordResponse
            {
                Id             = r.Id,
                AppointmentId  = r.AppointmentId,
                Notes          = r.Notes,
                Diagnosis      = r.Diagnosis,
                PrescriptionText = r.PrescriptionText,
                AttachmentsUrl = r.AttachmentsUrl,
                CreatedAt      = r.CreatedAtUtc
            })
            .ToListAsync(ct);

        return Ok(list);
    }

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
#endregion


