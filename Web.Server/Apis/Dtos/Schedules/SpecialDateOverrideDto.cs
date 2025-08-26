namespace Api.Endpoints.Dtos.Schedules;

public class SpecialDateOverrideDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public bool Closed { get; set; }
    public List<TimeRangeDto> Intervals { get; set; } = new();
}