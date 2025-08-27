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
    // -----------------------
    // Utilities
    // -----------------------
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
            Days = Enum.GetValues<DayOfWeek>().Select(d => new WorkingDay { Day = d }).ToList()
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
                // extend
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
        // subtract each break from each interval
        var result = new List<TimeRange>(sources);
        foreach (var b in subtracts.Where(ValidRange))
        {
            var next = new List<TimeRange>();
            foreach (var s in result)
            {
                if (b.To <= s.From || b.From >= s.To)
                {
                    next.Add(s); // no overlap
                    continue;
                }
                // overlap segments
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

        // effective = intervals − breaks
        return SubtractRanges(MergeRanges(day.Intervals), MergeRanges(day.Breaks));
    }

    private static bool Overlaps(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
        => aStart < bEnd && bStart < aEnd;

    private static TimeRangeDto ToDto(TimeRange r) => new(r.From.ToString("HH:mm"), r.To.ToString("HH:mm"));
    private static TimeRange ParseRange(TimeRangeDto dto) => new(TimeOnly.Parse(dto.From), TimeOnly.Parse(dto.To));

    // -----------------------
    // GET /api/schedule
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
}
