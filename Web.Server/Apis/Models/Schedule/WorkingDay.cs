using Api.Endpoints.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Models.Schedule;

[Owned]
public class WorkingDay
{
    public DayOfWeek Day { get; set; }
    public List<TimeRange> Intervals { get; set; } = new(); 
    public List<TimeRange> Breaks { get; set; } = new();    
    public bool Closed { get; set; } = false;
}