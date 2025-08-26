namespace Api.Endpoints.Dtos.Schedules;

public class WorkingDayDto
{
    public DayOfWeek Day { get; set; }
    public bool Closed { get; set; }
    public List<TimeRangeDto> Intervals { get; set; } = new();
    public List<TimeRangeDto> Breaks { get; set; } = new();
}