using Api.Endpoints.Context;
using Api.Endpoints.Dtos.Schedules;
using Api.Endpoints.Models.Apointments;
using Api.Endpoints.Models.Clinic;
using Api.Endpoints.Models.Schedule;
using Api.Endpoints.Models.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Controllers;

[ApiController]
[Route("api/schedule")]
[Authorize]
public class WorkScheduleController(AppDbContext db) : ControllerBase
{
    
    
    
    private async Task<WorkSchedule> EnsureScheduleAsync(CancellationToken ct)
    {
        var ws = await db.WorkSchedules
            .Include(x => x.Days).ThenInclude(d => d.Intervals)
            .Include(x => x.Days).ThenInclude(d => d.Breaks)
            .Include(x => x.Overrides)
            .FirstOrDefaultAsync(ct);

        if (ws is not null) return ws;

        ws = new WorkSchedule
        {
            Days = Enum.GetValues<DayOfWeek>().Select(d => new WorkingDay { Day = d  , Intervals = [new TimeRange(TimeOnly.Parse("12:45"),
                                                          TimeOnly.Parse("22:45"))]}).ToList(),
            
        };
        db.WorkSchedules.Add(ws);
        await db.SaveChangesAsync(ct);
        return ws;
    }

    private static bool ValidRange(TimeRange r) => r.From < r.To;

    private static List<TimeRange> MergeRanges(IEnumerable<TimeRange> ranges)
    {
        var list = ranges.Where(ValidRange)
                         .OrderBy(r => r.From)
                         .ThenBy(r => r.To)
                         .ToList();
        if (list.Count == 0) return new();

        var merged = new List<TimeRange> { list[0] };
        for (int i = 1; i < list.Count; i++)
        {
            var last = merged[^1];
            var cur  = list[i];
            if (cur.From <= last.To)
            {
                
                merged[^1] = new TimeRange(last.From, cur.To > last.To ? cur.To : last.To);
            }
            else
            {
                merged.Add(cur);
            }
        }
        return merged;
    }

    private static List<TimeRange> SubtractRanges(List<TimeRange> sources, List<TimeRange> subtracts)
    {
        
        var result = new List<TimeRange>(sources);
        foreach (var b in subtracts.Where(ValidRange))
        {
            var next = new List<TimeRange>();
            foreach (var s in result)
            {
                if (b.To <= s.From || b.From >= s.To)
                {
                    next.Add(s); 
                    continue;
                }
                
                if (b.From > s.From)
                    next.Add(new TimeRange(s.From, TimeOnly.MinValue.Add(s.From.ToTimeSpan().Add((b.From - s.From)))));

                if (b.To < s.To)
                    next.Add(new TimeRange(b.To, s.To));
            }
            result = MergeRanges(next);
        }
        return MergeRanges(result);
    }

    private static List<TimeRange> ResolveIntervalsForDate(WorkSchedule ws, DateOnly date)
    {
        var od = ws.Overrides.FirstOrDefault(o => o.Date == date);
        if (od is not null) return od.Closed ? new List<TimeRange>() : MergeRanges(od.Intervals);

        var day = ws.Days.FirstOrDefault(d => d.Day == date.DayOfWeek);
        if (day is null || day.Closed) return new List<TimeRange>();

        
        return SubtractRanges(MergeRanges(day.Intervals), MergeRanges(day.Breaks));
    }

    private static bool Overlaps(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
        => aStart < bEnd && bStart < aEnd;

    private static TimeRangeDto ToDto(TimeRange r) => new(r.From.ToString("HH:mm"), r.To.ToString("HH:mm"));
    private static TimeRange ParseRange(TimeRangeDto dto) => new(TimeOnly.Parse(dto.From), TimeOnly.Parse(dto.To));

    
    
    // -----------------------
    [HttpGet]
    public async Task<ActionResult<WorkScheduleResponse>> GetSchedule(CancellationToken ct)
    {
        var ws = await EnsureScheduleAsync(ct);

        var resp = new WorkScheduleResponse
        {
            Days = ws.Days.OrderBy(d => d.Day)
                .Select(d => new WorkingDayDto
                {
                    Day = d.Day,
                    Closed = d.Closed,
                    Intervals = MergeRanges(d.Intervals).Select(ToDto).ToList(),
                    Breaks   = MergeRanges(d.Breaks).Select(ToDto).ToList()
                }).ToList(),
            Overrides = ws.Overrides.OrderBy(o => o.Date)
                .Select(o => new SpecialDateOverrideDto
                {
                    Id = o.Id,
                    Date = o.Date,
                    Closed = o.Closed,
                    Intervals = MergeRanges(o.Intervals).Select(ToDto).ToList()
                }).ToList()
        };

        return Ok(resp);
    }

    // -----------------------
    // PUT /api/schedule
    // -----------------------
    [Authorize(Roles = "Doctor,Secretary")]
    [HttpPut]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateWorkScheduleRequest body, CancellationToken ct)
    {
        var ws = await EnsureScheduleAsync(ct);

        // replace Days
        ws.Days = (body.Days ?? new()).Select(d => new WorkingDay
        {
            Day       = d.Day,
            Closed    = d.Closed,
            Intervals = (d.Intervals ?? new()).Select(ParseRange).Where(ValidRange).ToList(),
            Breaks    = (d.Breaks ?? new()).Select(ParseRange).Where(ValidRange).ToList()
        }).ToList();

        // replace Overrides
        ws.Overrides = (body.Overrides ?? new()).Select(o => new SpecialDateOverride
        {
            Id        = o.Id == Guid.Empty ? Guid.NewGuid() : o.Id,
            Date      = o.Date,
            Closed    = o.Closed,
            Intervals = (o.Intervals ?? new()).Select(ParseRange).Where(ValidRange).ToList()
        }).ToList();

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

[HttpGet("slots/summary")]
public async Task<ActionResult<SlotsSummaryDto>> GetSlotsSummary(
    [FromQuery] DateOnly date,
    [FromQuery] Guid serviceId,
    CancellationToken ct)
{
    var settings = await db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();
    var service  = await db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == serviceId, ct);
    if (service is null) return NotFound(new { message = "Service not found." });

    var ws        = await EnsureScheduleAsync(ct);
    var effective = ResolveIntervalsForDate(ws, date); // working intervals after overrides & breaks

    var resp = new SlotsSummaryDto();
    if (effective.Count == 0) return Ok(resp); // empty day

    var duration = TimeSpan.FromMinutes(service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes);
    var buffer   = TimeSpan.FromMinutes(settings.BufferBetweenVisitsMinutes);

    // Booked items (Booked only)
    var booked = await db.Appointments.AsNoTracking()
        .Where(a => a.Date == date && a.ServiceId == serviceId && a.Status == AppointmentStatus.Booked)
        .Select(a => new { a.Start, a.End })
        .ToListAsync(ct);

    // 1) StartTimes (same logic as your /slots endpoint)
    resp.StartTimes = BuildStartSlots(effective, booked.Select(b => (b.Start, b.End)).ToList(), duration, buffer);

    // 2) Booked as ranges (exact, no buffer)
    resp.Booked = booked
        .Select(b => new TimeRangeDto(b.Start.ToString("HH:mm"), b.End.ToString("HH:mm")))
        .OrderBy(x => x.From)
        .ToList();

    // 3) FreeRanges = effective - (booked expanded by buffer on both sides)
    var bookedWithBuffer = booked
        .Select(b =>
        {
            var from = ClampToDay(TimeOnly.FromTimeSpan(b.Start.ToTimeSpan() - buffer));
            var to   = ClampToDay(TimeOnly.FromTimeSpan(b.End.ToTimeSpan()   + buffer));
            return new TimeRange(from, to);
        })
        .ToList();

    var free = SubtractRanges(effective, MergeRanges(bookedWithBuffer));   // reuse your utilities
    resp.FreeRanges = free.Select(ToDto).ToList();

    return Ok(resp);
}


    // -----------------------
    // GET /api/schedule/slots?date=YYYY-MM-DD&serviceId=GUID
    // -----------------------
    [HttpGet("slots")]
    public async Task<ActionResult<List<string>>> GetSlots([FromQuery] DateOnly date, [FromQuery] Guid serviceId, CancellationToken ct)
    {
        var settings = await db.ClinicSettings.AsNoTracking().FirstOrDefaultAsync(ct) ?? new ClinicSettings();
        var service  = await db.MedicalServices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == serviceId, ct);
        if (service is null) return NotFound(new { message = "Service not found." });

        var ws = await EnsureScheduleAsync(ct);
        var effective = ResolveIntervalsForDate(ws, date);
        if (effective.Count == 0) return Ok(new List<string>());

        var duration = TimeSpan.FromMinutes(service.VisitMinutes > 0 ? service.VisitMinutes : settings.DefaultVisitMinutes);
        var buffer   = TimeSpan.FromMinutes(settings.BufferBetweenVisitsMinutes);

        // load booked (Booked only)
        var existing = await db.Appointments.AsNoTracking()
            .Where(a => a.Date == date && a.ServiceId == serviceId && a.Status == AppointmentStatus.Booked)
            .Select(a => new { a.Start, a.End })
            .ToListAsync(ct);

        var slots = new List<string>();

        foreach (var r in effective)
        {
            var s = r.From.ToTimeSpan();
            var e = r.To.ToTimeSpan();

            while (s + duration <= e)
            {
                var candStart = s;
                var candEnd   = s + duration;

                var hasOverlap = existing.Any(a => Overlaps(candStart, candEnd, a.Start.ToTimeSpan(), a.End.ToTimeSpan()));
                if (!hasOverlap)
                    slots.Add(TimeOnly.FromTimeSpan(candStart).ToString("HH:mm"));

                s += (duration + buffer);
            }
        }

        return Ok(slots.Distinct().OrderBy(x => x).ToList());
    }

    private static string HHmm(TimeSpan ts)
        => TimeOnly.FromTimeSpan(ts).ToString("HH:mm");

    private static List<string> BuildStartSlots(List<TimeRange> effective, List<(TimeOnly Start, TimeOnly End)> existing, TimeSpan duration, TimeSpan buffer)
    {
        var slots = new List<string>();

        foreach (var r in effective)
        {
            var s = r.From.ToTimeSpan();
            var e = r.To.ToTimeSpan();

            while (s + duration <= e)
            {
                var candStart = s;
                var candEnd   = s + duration;

                var hasOverlap = existing.Any(a => Overlaps(candStart, candEnd, a.Start.ToTimeSpan(), a.End.ToTimeSpan()));

                if (!hasOverlap)
                    slots.Add(HHmm(candStart));

                s += (duration + buffer);
            }
        }

        return slots.Distinct().OrderBy(x => x).ToList();
    }

    private static TimeOnly ClampToDay(TimeOnly t)
    {
        // TimeOnly is already a time-of-day; clamp defensively if needed
        if (t < TimeOnly.MinValue) return TimeOnly.MinValue;
        if (t > TimeOnly.MaxValue) return TimeOnly.MaxValue;
        return t;
    }

}
