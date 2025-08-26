using Api.Endpoints.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Models.Schedule;

[Owned]
public class WorkingDay
{
    public DayOfWeek Day { get; set; }
    public List<TimeRange> Intervals { get; set; } = new(); // e.g., 09:00-13:00, 15:00-18:00
    public List<TimeRange> Breaks { get; set; } = new();    // optional breaks inside day
    public bool Closed { get; set; } = false;
}