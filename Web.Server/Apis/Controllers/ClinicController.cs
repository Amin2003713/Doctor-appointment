using System.Security.Claims;
using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Clinic;
using Api.Endpoints.Dtos.doctor;
using Api.Endpoints.Dtos.Schedules;
using Api.Endpoints.Dtos.Services;
using Api.Endpoints.Models.Apointments;
using Api.Endpoints.Models.Clinic;
using Api.Endpoints.Models.Doctores;
using Api.Endpoints.Models.MediaclService;
using Api.Endpoints.Models.Schedule;
using Api.Endpoints.Models.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// NOTE: assumes you already have these domain models & DbContext from earlier:
// - ClinicSettings, PaymentMethods
// - MedicalService (with Money Price), WorkSchedule, WorkingDay, SpecialDateOverride, TimeRange
// - AppDbContext configured with TimeOnly<->TimeSpan converters for TimeRange

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/clinic")]
public class ClinicController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClinicController(AppDbContext db)
    {
        _db = db;
    }

    // -----------------------
    // Clinic Settings
    // -----------------------
    [HttpGet("settings")]
    public async Task<ActionResult<ClinicSettingsResponse>> GetSettings(CancellationToken ct)
    {
        var s = await _db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings(); // default empty if not set yet

        return Ok(new ClinicSettingsResponse
        {
            Name = s.Name,
            Address = s.Address,
            PhoneNumber = s.PhoneNumber,
            Email = s.Email,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            Notes = s.Notes,
            AcceptedPayments = s.AcceptedPayments,
            DefaultVisitMinutes = s.DefaultVisitMinutes,
            BufferBetweenVisitsMinutes = s.BufferBetweenVisitsMinutes
        });
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings([FromBody] UpdateClinicSettingsRequest body, CancellationToken ct)
    {
        // Optional validation (if validator is registered)


        var s = await _db.ClinicSettings.FirstOrDefaultAsync(ct);

        if (s is null)
        {
            s = new ClinicSettings();
            _db.ClinicSettings.Add(s);
        }

        s.Name = body.Name;
        s.Address = body.Address;
        s.PhoneNumber = body.PhoneNumber;
        s.Email = body.Email;
        s.Latitude = body.Latitude;
        s.Longitude = body.Longitude;
        s.Notes = body.Notes;
        s.AcceptedPayments = body.AcceptedPayments;
        s.DefaultVisitMinutes = body.DefaultVisitMinutes;
        s.BufferBetweenVisitsMinutes = body.BufferBetweenVisitsMinutes;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // -----------------------
    // Services (catalog)
    // -----------------------
    [HttpGet("services")]
    public async Task<ActionResult<List<ServiceResponse>>> ListServices(CancellationToken ct)
    {
        var items = await _db.MedicalServices.AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(s => new ServiceResponse
            {
                Id = s.Id,
                Code = s.Code,
                Title = s.Title,
                Description = s.Description,
                PriceAmount = s.Price.Amount,
                PriceCurrency = s.Price.Currency,
                VisitMinutes = s.VisitMinutes,
                IsActive = s.IsActive
            })
            .ToListAsync(ct);

        return Ok(items);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("services")]
    public async Task<ActionResult<Guid>> CreateService([FromBody] UpsertServiceRequest body, CancellationToken ct)
    {
        // Ensure unique code
        var exists = await _db.MedicalServices.AnyAsync(x => x.Code == body.Code, ct);
        if (exists)
            return Conflict(new
            {
                message = "Service code already exists."
            });

        var entity = new MedicalService
        {
            Code = body.Code.Trim(),
            Title = body.Title.Trim(),
            Description = body.Description?.Trim(),
            Price = new Money(body.PriceAmount, body.PriceCurrency),
            VisitMinutes = body.VisitMinutes,
            IsActive = body.IsActive
        };

        _db.MedicalServices.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Ok(entity.Id);
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("services/{id:guid}")]
    public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpsertServiceRequest body, CancellationToken ct)
    {
        var entity = await _db.MedicalServices.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        // If code changed, enforce uniqueness
        if (!string.Equals(entity.Code, body.Code, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _db.MedicalServices.AnyAsync(x => x.Code == body.Code && x.Id != id, ct);
            if (exists)
                return Conflict(new
                {
                    message = "Service code already exists."
                });
        }

        entity.Code = body.Code.Trim();
        entity.Title = body.Title.Trim();
        entity.Description = body.Description?.Trim();
        entity.Price = new Money(body.PriceAmount, body.PriceCurrency);
        entity.VisitMinutes = body.VisitMinutes;
        entity.IsActive = body.IsActive;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor")]
    [HttpDelete("services/{id:guid}")]
    public async Task<IActionResult> DeleteService(Guid id, CancellationToken ct)
    {
        var entity = await _db.MedicalServices.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        _db.MedicalServices.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // -----------------------
    // Work Schedule
    // -----------------------
    [HttpGet("schedule")]
    public async Task<ActionResult<WorkScheduleResponse>> GetSchedule(CancellationToken ct)
    {
        var ws = await _db.WorkSchedules
                     .Include(x => x.Overrides)
                     .AsNoTracking()
                     .FirstOrDefaultAsync(ct) ??
                 new WorkSchedule();

        var resp = new WorkScheduleResponse
        {
            Days = ws.Days.OrderBy(d => d.Day)
                .Select(d => new WorkingDayDto
                {
                    Day = d.Day,
                    Closed = d.Closed,
                    Intervals = d.Intervals.Select(ToDto).ToList(),
                    Breaks = d.Breaks.Select(ToDto).ToList()
                })
                .ToList(),
            Overrides = ws.Overrides.OrderBy(o => o.Date)
                .Select(o => new SpecialDateOverrideDto
                {
                    Id = o.Id,
                    Date = o.Date,
                    Closed = o.Closed,
                    Intervals = o.Intervals.Select(ToDto).ToList()
                })
                .ToList()
        };

        return Ok(resp);

        static TimeRangeDto ToDto(TimeRange r)
        {
            return new TimeRangeDto(r.From.ToString("HH:mm"), r.To.ToString("HH:mm"));
        }
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("schedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateWorkScheduleRequest body, CancellationToken ct)
    {
        var ws = await _db.WorkSchedules.Include(x => x.Overrides).FirstOrDefaultAsync(ct);

        if (ws is null)
        {
            ws = new WorkSchedule();
            _db.WorkSchedules.Add(ws);
        }

        // Full replace strategy (simple & clear)
        ws.Days = body.Days.Select(d => new WorkingDay
            {
                Day = d.Day,
                Closed = d.Closed,
                Intervals = d.Intervals.Select(ParseRange).ToList(),
                Breaks = d.Breaks.Select(ParseRange).ToList()
            })
            .ToList();

        ws.Overrides = body.Overrides.Select(o => new SpecialDateOverride
            {
                Id = o.Id == Guid.Empty ? Guid.NewGuid() : o.Id,
                Date = o.Date,
                Closed = o.Closed,
                Intervals = o.Intervals.Select(ParseRange).ToList()
            })
            .ToList();

        await _db.SaveChangesAsync(ct);
        return NoContent();

        static TimeRange ParseRange(TimeRangeDto dto)
        {
            return new TimeRange(TimeOnly.Parse(dto.From), TimeOnly.Parse(dto.To));
        }
    }

    // ---------- SLOTS (override: subtract existing appointments) ----------
    // GET /api/clinic/slots?date=2025-09-03&serviceId=GUID
    [HttpGet("slots")]
    public async Task<ActionResult<List<string>>> GetSlots([FromQuery] DateOnly date, [FromQuery] Guid serviceId, CancellationToken ct)
    {
        var settings = await _db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();
        var service  = await _db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == serviceId, ct);
        if (service is null)
            return NotFound(new
            {
                message = "Service not found."
            });

        var ws        = await _db.WorkSchedules.Include(x => x.Overrides).AsNoTracking().FirstOrDefaultAsync(ct) ?? new WorkSchedule();
        var intervals = ResolveIntervalsForDate(ws, date);
        if (intervals.Count == 0) return Ok(new List<string>());

        var duration = TimeSpan.FromMinutes(service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes);
        var buffer   = TimeSpan.FromMinutes(settings.BufferBetweenVisitsMinutes);

        // load booked (not cancelled/noshow) for that date/service
        var existing = await _db.Appointments.AsNoTracking()
            .Where(a => a.Date == date && a.ServiceId == serviceId && a.Status == AppointmentStatus.Booked)
            .Select(a => new
            {
                a.Start,
                a.End
            })
            .ToListAsync(ct);

        var slots = new List<string>();

        foreach (var rng in intervals)
        {
            var s = rng.From.ToTimeSpan();
            var e = rng.To.ToTimeSpan();

            while (s + duration <= e)
            {
                var candStart = s;
                var candEnd   = s + duration;

                // overlap with existing?
                var overlaps = existing.Any(a => Overlaps(candStart, candEnd, a.Start.ToTimeSpan(), a.End.ToTimeSpan()));
                if (!overlaps)
                    slots.Add(TimeOnly.FromTimeSpan(candStart).ToString("HH:mm"));

                s += duration + buffer;
            }
        }

        return Ok(slots.Distinct().OrderBy(x => x).ToList());
    }

    // -------------------------------------------------
    // 2) APPOINTMENTS
    // -------------------------------------------------

    // Role matrix:
    // - Patient: can create/list their own appointments; can cancel their own
    // - Secretary: can create/list/update/cancel ANY appointment
    // - Doctor: same as Secretary

    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpPost("appointments")]
    public async Task<ActionResult<Guid>> CreateAppointment([FromBody] UpsertAppointmentRequest body, CancellationToken ct)
    {
        // basic validations
        if (body.ServiceId == Guid.Empty) return BadRequest("ServiceId required.");
        if (string.IsNullOrWhiteSpace(body.Start)) return BadRequest("Start required (HH:mm).");

        var service = await _db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == body.ServiceId, ct);
        if (service is null) return NotFound("Service not found.");

        var settings = await _db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();

        // Who is creating?
        var (userId, userRole) = GetUserIdAndRole();

        // If Secretary/Doctor, patient info is required
        string patientName;
        string patientPhone;
        Guid?  patientUserId = null;

        if (userRole is "Secretary" or "Doctor")
        {
            if (string.IsNullOrWhiteSpace(body.PatientFullName) || string.IsNullOrWhiteSpace(body.PatientPhone))
                return BadRequest("PatientFullName and PatientPhone are required when booking as Secretary/Doctor.");

            patientName  = body.PatientFullName.Trim();
            patientPhone = body.PatientPhone.Trim();
        }
        else
        {
            // Patient booking for themselves (name/phone can be optional; keep a fallback)
            patientUserId = userId;
            patientName  = string.IsNullOrWhiteSpace(body.PatientFullName) ? "Patient" : body.PatientFullName.Trim();
            patientPhone = string.IsNullOrWhiteSpace(body.PatientPhone) ? "-" : body.PatientPhone.Trim();
        }

        // Validate time is within schedule and slot is available
        var ws = await _db.WorkSchedules.Include(x => x.Overrides).AsNoTracking().FirstOrDefaultAsync(ct) ?? new WorkSchedule();

        var intervals = ResolveIntervalsForDate(ws, body.Date);
        if (intervals.Count == 0) return BadRequest("Clinic is closed for the selected date.");

        var start    = TimeOnly.Parse(body.Start);
        var duration = TimeSpan.FromMinutes(service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes);
        var buffer   = TimeSpan.FromMinutes(settings.BufferBetweenVisitsMinutes);
        var end      = start.Add(duration);

        // 1) time inside any interval?
        var inside = intervals.Any(tr => start >= tr.From && end <= tr.To);
        if (!inside) return BadRequest("Selected time is outside working intervals.");

        // 2) align with generated slots (optional strictness). Here we only check for overlap with booked.
        var booked = await _db.Appointments.AsNoTracking()
            .Where(a => a.Date == body.Date && a.ServiceId == body.ServiceId && a.Status == AppointmentStatus.Booked)
            .ToListAsync(ct);

        var conflict = booked.Any(a => Overlaps(start.ToTimeSpan(), end.ToTimeSpan(), a.Start.ToTimeSpan(), a.End.ToTimeSpan()));
        if (conflict) return Conflict("Selected time overlaps with an existing appointment.");

        // snapshot price
        var entity = new Appointment
        {
            ServiceId = body.ServiceId,
            PatientUserId = patientUserId,
            PatientFullName = patientName,
            PatientPhone = patientPhone,
            Date = body.Date,
            Start = start,
            End = end,
            PriceAmount = service.Price.Amount,
            PriceCurrency = service.Price.Currency,
            Notes = body.Notes,
            Status = AppointmentStatus.Booked,
            CreatedByUserId = userId
        };

        _db.Appointments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return Ok(entity.Id);
    }

    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpGet("appointments")]
    public async Task<ActionResult<List<AppointmentResponse>>> ListAppointments(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] Guid? patientUserId,
        CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();

        var q = _db.Appointments.AsNoTracking().AsQueryable();

        // Visibility
        if (role == "Patient")
        {
            q = q.Where(a => a.PatientUserId == uid);
        }
        else
        {
            if (patientUserId.HasValue)
                q = q.Where(a => a.PatientUserId == patientUserId.Value);
        }

        if (from.HasValue) q = q.Where(a => a.Date >= from.Value);
        if (to.HasValue)   q = q.Where(a => a.Date <= to.Value);

        var services = await _db.MedicalServices.AsNoTracking().ToDictionaryAsync(s => s.Id, s => s.Title, ct);

        var list = await q.OrderBy(a => a.Date)
            .ThenBy(a => a.Start)
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                ServiceId = a.ServiceId,
                ServiceTitle = "",
                Date = a.Date,
                Start = a.Start.ToString("HH:mm"),
                End = a.End.ToString("HH:mm"),
                Status = a.Status,
                PatientFullName = a.PatientFullName,
                PatientPhone = a.PatientPhone,
                PriceAmount = a.PriceAmount,
                PriceCurrency = a.PriceCurrency,
                Notes = a.Notes
            })
            .ToListAsync(ct);

        // fill service titles
        foreach (var r in list)
        {
            if (services.TryGetValue(r.ServiceId, out var t))
                r.ServiceTitle = t;
        }

        return Ok(list);
    }

    [Authorize(Roles = "Doctor,Secretary,Patient")]
    [HttpPut("appointments/{id:guid}/cancel")]
    public async Task<IActionResult> CancelAppointment(Guid id, CancellationToken ct)
    {
        var (uid, role) = GetUserIdAndRole();
        var ap = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();

        if (role == "Patient" && ap.PatientUserId != uid)
            return Forbid();

        if (ap.Status != AppointmentStatus.Booked)
            return BadRequest("Only Booked appointments can be cancelled.");

        ap.Status = AppointmentStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("appointments/{id:guid}/complete")]
    public async Task<IActionResult> CompleteAppointment(Guid id, CancellationToken ct)
    {
        var ap = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();
        if (ap.Status != AppointmentStatus.Booked)
            return BadRequest("Only Booked appointments can be completed.");

        ap.Status = AppointmentStatus.Completed;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("appointments/{id:guid}/reschedule")]
    public async Task<IActionResult> RescheduleAppointment(Guid id, [FromBody] UpsertAppointmentRequest body, CancellationToken ct)
    {
        var ap = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (ap is null) return NotFound();
        if (ap.Status != AppointmentStatus.Booked)
            return BadRequest("Only Booked appointments can be rescheduled.");

        if (ap.ServiceId != body.ServiceId) return BadRequest("Cannot change service here.");

        var service  = await _db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == ap.ServiceId, ct)!;
        var settings = await _db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();
        var ws       = await _db.WorkSchedules.Include(x => x.Overrides).AsNoTracking().FirstOrDefaultAsync(ct) ?? new WorkSchedule();

        var intervals = ResolveIntervalsForDate(ws, body.Date);
        if (intervals.Count == 0) return BadRequest("Clinic is closed for the selected date.");

        var duration = TimeSpan.FromMinutes(service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes);

        var newStart = TimeOnly.Parse(body.Start);
        var newEnd   = newStart.Add(duration);

        var inside = intervals.Any(tr => newStart >= tr.From && newEnd <= tr.To);
        if (!inside) return BadRequest("Selected time is outside working intervals.");

        var booked = await _db.Appointments.AsNoTracking()
            .Where(a => a.Date == body.Date && a.ServiceId == ap.ServiceId && a.Status == AppointmentStatus.Booked && a.Id != ap.Id)
            .ToListAsync(ct);

        var conflict = booked.Any(a => Overlaps(newStart.ToTimeSpan(), newEnd.ToTimeSpan(), a.Start.ToTimeSpan(), a.End.ToTimeSpan()));
        if (conflict) return Conflict("Selected time overlaps with an existing appointment.");

        ap.Date  = body.Date;
        ap.Start = newStart;
        ap.End   = newEnd;
        ap.Notes = body.Notes;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // -------------------------------------------------
    // 3) DOCTOR PROFILE / INTRODUCTION
    // -------------------------------------------------

    [HttpGet("doctor/profile")]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfile(CancellationToken ct)
    {
        var p = await _db.DoctorProfiles.AsNoTracking().FirstOrDefaultAsync(ct);

        if (p is null)
        {
            // default empty
            return Ok(new DoctorProfileResponse
            {
                FullName = "",
                Title = "",
                Biography = "",
                Specialties = Array.Empty<string>(),
                Education = Array.Empty<string>(),
                Languages = Array.Empty<string>(),
                YearsOfExperience = 0
            });
        }

        return Ok(new DoctorProfileResponse
        {
            FullName = p.FullName,
            Title = p.Title,
            Biography = p.Biography,
            Specialties = p.Specialties,
            Education = p.Education,
            Languages = p.Languages,
            YearsOfExperience = p.YearsOfExperience,
            PhotoUrl = p.PhotoUrl,
            Website = p.Website,
            Instagram = p.Instagram,
            LinkedIn = p.LinkedIn,
            WhatsApp = p.WhatsApp
        });
    }

    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut("doctor/profile")]
    public async Task<IActionResult> UpsertDoctorProfile([FromBody] UpsertDoctorProfileRequest body, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(body.FullName))
            return BadRequest("FullName is required.");

        var p = await _db.DoctorProfiles.FirstOrDefaultAsync(ct);

        if (p is null)
        {
            p = new DoctorProfile();
            _db.DoctorProfiles.Add(p);
        }

        p.FullName = body.FullName.Trim();
        p.Title = (body.Title ?? "").Trim();
        p.Biography = body.Biography?.Trim() ?? "";
        p.Specialties = body.Specialties ?? Array.Empty<string>();
        p.Education = body.Education ?? Array.Empty<string>();
        p.Languages = body.Languages ?? Array.Empty<string>();
        p.YearsOfExperience = Math.Max(0, body.YearsOfExperience);
        p.PhotoUrl = body.PhotoUrl?.Trim();
        p.Website = body.Website?.Trim();
        p.Instagram = body.Instagram?.Trim();
        p.LinkedIn = body.LinkedIn?.Trim();
        p.WhatsApp = body.WhatsApp?.Trim();

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // -------------------------------------------------
    // Helpers
    // -------------------------------------------------
    private static bool Overlaps(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
    {
        return aStart < bEnd && bStart < aEnd;
    }

    private static List<TimeRange> ResolveIntervalsForDate(WorkSchedule ws, DateOnly date)
    {
        var od = ws.Overrides.FirstOrDefault(o => o.Date == date);
        if (od is not null) return od.Closed ? new List<TimeRange>() : od.Intervals;

        var day = ws.Days.FirstOrDefault(d => d.Day == date.DayOfWeek);
        if (day is null || day.Closed) return new List<TimeRange>();

        return day.Intervals;
    }

    private (Guid? userId, string role) GetUserIdAndRole()
    {
        var   role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? User.FindFirstValue("roles") ?? "";
        var   sub  = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        Guid? uid  = null;
        if (Guid.TryParse(sub, out var parsed)) uid = parsed;
        return (uid, role);
    }
}