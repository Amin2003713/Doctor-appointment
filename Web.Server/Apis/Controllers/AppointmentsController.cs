using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.doctor;
using Api.Endpoints.Models.Appointments;
using Api.Endpoints.Models.Clinic;
using Api.Endpoints.Models.Schedule;
using Api.Endpoints.Models.User;
using Api.Endpoints.Models.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController(
    AppDbContext         db,
    UserManager<AppUser> userManager
) : ControllerBase
{
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] UpsertAppointmentRequest body, CancellationToken ct)
    {
        if (body.ServiceId == Guid.Empty) return BadRequest("ServiceId required.");
        if (string.IsNullOrWhiteSpace(body.Start)) return BadRequest("Start required (HH:mm).");

        var service = await db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == body.ServiceId, ct);

        if (service is null) return NotFound("Service not found.");

        var settings = await db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();

        var (userId, role) = GetUserIdAndRole();


        string patientName;
        string patientPhone;
        long?  patientUserId = (await userManager.FindByNameAsync(body.PatientPhone!))?.Id;

        if (!patientUserId.HasValue)
            return BadRequest("patient not Found.");

        if (role is "Secretary" or "Doctor")
        {
            if (string.IsNullOrWhiteSpace(body.PatientFullName) || string.IsNullOrWhiteSpace(body.PatientPhone))
                return BadRequest("PatientFullName and PatientPhone are required when booking as Secretary/Doctor.");

            patientName  = body.PatientFullName.Trim();
            patientPhone = body.PatientPhone.Trim();
        }
        else
        {
            patientUserId = userId;
            patientName   = string.IsNullOrWhiteSpace(body.PatientFullName) ? "Patient" : body.PatientFullName.Trim();
            patientPhone  = string.IsNullOrWhiteSpace(body.PatientPhone) ? "-" : body.PatientPhone.Trim();
        }


        var schedule = await db.WorkSchedules.AsNoTracking().Include(x => x.Overrides).FirstOrDefaultAsync(ct) ??
                       new WorkSchedule();

        var intervals = ResolveIntervalsForDate(schedule, body.Date);
        if (intervals.Count == 0) return BadRequest("Clinic is closed for the selected date.");

        var start        = TimeOnly.Parse(body.Start);
        var visitMinutes = service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes;
        var end          = start.AddMinutes(visitMinutes);

        var insideWorkingHours = intervals.Any(tr => start >= tr.From && end <= tr.To);
        if (!insideWorkingHours) return BadRequest("Selected time is outside working hours.");


        var serviceOverlap = await db.Appointments.AsNoTracking().
                                      AnyAsync(a =>
                                                   a.Date      == body.Date                &&
                                                   a.ServiceId == body.ServiceId           &&
                                                   a.Status    == AppointmentStatus.Booked &&
                                                   // overlap check: start < a.End && a.Start < end
                                                   start   < a.End &&
                                                   a.Start < end,
                                               ct);


        if (serviceOverlap) return Conflict("Selected time overlaps another appointment for this service.");


        var patientOverlap = await db.Appointments.AsNoTracking().
                                      AnyAsync(a =>
                                                   a.Date   == body.Date                &&
                                                   a.Status == AppointmentStatus.Booked &&
                                                   (
                                                       (patientUserId.HasValue  && a.PatientUserId == patientUserId) ||
                                                       (!patientUserId.HasValue && a.PatientUserId == null && a.PatientPhone == patientPhone)
                                                   ) &&
                                                   // overlap check
                                                   start   < a.End &&
                                                   a.Start < end,
                                               ct);

        if (patientOverlap) return Conflict("This patient already has an appointment that overlaps this time.");


        var ap = new Appointment
        {
            ServiceId       = body.ServiceId,
            PatientUserId   = patientUserId ?? 0,
            PatientFullName = patientName,
            PatientPhone    = patientPhone,
            Date            = body.Date,
            Start           = start,
            End             = end,
            PriceAmount     = service.Price.Amount,
            PriceCurrency   = string.IsNullOrWhiteSpace(service.Price.Currency) ? "IRR" : service.Price.Currency,
            Notes           = body.Notes,
            Status          = AppointmentStatus.Booked,
            CreatedByUserId = userId
        };

        db.Appointments.Add(ap);
        await db.SaveChangesAsync(ct);
        return Ok(ap.Id);
    }


    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet]
    public async Task<ActionResult<List<AppointmentResponse>>> List(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] long?     patientUserId,
        CancellationToken     ct)
    {
        var (uid, role) = GetUserIdAndRole();

        var q = db.Appointments.AsNoTracking().AsQueryable();

        if (role == "Patient")
        {
            q = q.Where(a => a.PatientUserId == uid);
        }
        else if (patientUserId.HasValue)
        {
            q = q.Where(a => a.PatientUserId == patientUserId.Value);
        }

        if (from.HasValue) q = q.Where(a => a.Date >= from.Value);
        if (to.HasValue) q   = q.Where(a => a.Date <= to.Value);


        var services = await db.MedicalServices.AsNoTracking().ToDictionaryAsync(s => s.Id, s => s.Title, ct);

        var list = await q.OrderBy(a => a.Date).
                           ThenBy(a => a.Start).
                           Select(a => new AppointmentResponse
                           {
                               Id              = a.Id,
                               ServiceId       = a.ServiceId,
                               ServiceTitle    = "",
                               Date            = a.Date,
                               Start           = a.Start.ToString("HH:mm"),
                               End             = a.End.ToString("HH:mm"),
                               Status          = a.Status,
                               PatientFullName = a.PatientFullName,
                               PatientPhone    = a.PatientPhone,
                               PriceAmount     = a.PriceAmount,
                               PriceCurrency   = a.PriceCurrency,
                               Notes           = a.Notes,
                               PatientId       = a.PatientUserId ?? 0
                           }).
                           ToListAsync(ct);

        foreach (var r in list)
            if (services.TryGetValue(r.ServiceId, out var name))
                r.ServiceTitle = name;

        return Ok(list);
    }


    // Get by id
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AppointmentResponse>> GetById(Guid id, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();

        var a = await db.Appointments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (a is null) return NotFound();

        if (role == "Patient" && a.PatientUserId != uid) return Forbid();

        var svc = await db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(s => s.Id == a.ServiceId, ct);

        return Ok(new AppointmentResponse
        {
            Id              = a.Id,
            ServiceId       = a.ServiceId,
            ServiceTitle    = svc!.Code + " " + svc.Title ?? "",
            Date            = a.Date,
            Start           = a.Start.ToString("HH:mm"),
            End             = a.End.ToString("HH:mm"),
            Status          = a.Status,
            PatientFullName = a.PatientFullName,
            PatientPhone    = a.PatientPhone,
            PriceAmount     = a.PriceAmount,
            PriceCurrency   = a.PriceCurrency,
            Notes           = a.Notes,
            PatientId       = a.PatientUserId ?? 0

        });
    }

    // ---------------------------
    // Cancel
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();
        var ap = await db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();

        if (role == "Patient" && ap.PatientUserId != uid) return Forbid();
        if (ap.Status != AppointmentStatus.Booked) return BadRequest("Only Booked can be canceled.");

        ap.Status = AppointmentStatus.Cancelled; // ✅ use one spelling consistently
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // ---------------------------
    // Complete
    // ---------------------------
    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        var ap = await db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();
        if (ap.Status != AppointmentStatus.Booked) return BadRequest("Only Booked can be completed.");

        ap.Status = AppointmentStatus.Completed;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("{id:guid}/reschedule")]
    public async Task<IActionResult> Reschedule(Guid id, [FromBody] UpsertAppointmentRequest body, CancellationToken ct)
    {
        var ap = await db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();
        if (ap.Status    != AppointmentStatus.Booked) return BadRequest("Only Booked can be rescheduled.");
        if (ap.ServiceId != body.ServiceId) return BadRequest("Service cannot be changed here.");

        var service  = await db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == ap.ServiceId, ct)!;
        var settings = await db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct)                          ?? new ClinicSettings();
        var schedule = await db.WorkSchedules.AsNoTracking().Include(x => x.Overrides).FirstOrDefaultAsync(ct) ?? new WorkSchedule();

        var intervals = ResolveIntervalsForDate(schedule, body.Date);
        if (intervals.Count == 0) return BadRequest("Clinic is closed for selected date.");

        var visitMinutes = service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes;
        var newStart     = TimeOnly.Parse(body.Start);
        var newEnd       = newStart.AddMinutes(visitMinutes);

        var inside = intervals.Any(tr => newStart >= tr.From && newEnd <= tr.To);
        if (!inside) return BadRequest("Selected time is outside working hours.");


        var svcConflict = await db.Appointments.AsNoTracking().
                                   AnyAsync(a =>
                                                a.Date      == body.Date                &&
                                                a.ServiceId == ap.ServiceId             &&
                                                a.Status    == AppointmentStatus.Booked &&
                                                a.Id        != ap.Id                    &&
                                                // overlap check
                                                newStart < a.End &&
                                                a.Start  < newEnd,
                                            ct);

        if (svcConflict) return Conflict("Selected time overlaps another appointment for this service.");

        // NEW RULE: patient-level overlap across ANY service (exclude current ap)
        var patientConflict = await db.Appointments.AsNoTracking().
                                       AnyAsync(a =>
                                                    a.Date   == body.Date                &&
                                                    a.Status == AppointmentStatus.Booked &&
                                                    a.Id     != ap.Id                    &&
                                                    (
                                                        (ap.PatientUserId.HasValue  && a.PatientUserId == ap.PatientUserId) ||
                                                        (!ap.PatientUserId.HasValue && a.PatientUserId == null && a.PatientPhone == ap.PatientPhone)
                                                    ) &&
                                                    // overlap check
                                                    newStart < a.End &&
                                                    a.Start  < newEnd,
                                                ct);

        if (patientConflict) return Conflict("This patient already has an appointment that overlaps this time.");

        ap.Date  = body.Date;
        ap.Start = newStart;
        ap.End   = newEnd;
        ap.Notes = body.Notes;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    // ---------------------------
    // Helpers
    // ---------------------------
    private static bool Overlaps(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
        => aStart < bEnd && bStart < aEnd;

    private static List<TimeRange> ResolveIntervalsForDate(WorkSchedule ws, DateOnly date)
    {
        var od = ws.Overrides.FirstOrDefault(o => o.Date == date);
        if (od is not null) return od.Closed ? new List<TimeRange>() : od.Intervals;

        var day = ws.Days.FirstOrDefault(d => d.Day == date.DayOfWeek);
        if (day is null || day.Closed) return new List<TimeRange>();

        return day.Intervals;
    }

    private(long? userId, string role) GetUserIdAndRole()
    {
        var   role                                  = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? User.FindFirstValue("roles") ?? "";
        var   sub                                   = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        long? uid                                   = null;
        if (long.TryParse(sub, out var parsed)) uid = parsed;
        return (uid, role);
    }
}