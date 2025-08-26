using Api.Endpoints.Models.ValueObjects;

namespace Api.Endpoints.Models.Schedule;

public class SpecialDateOverride
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly Date { get; set; }
    public bool Closed { get; set; } = false;
    public List<TimeRange> Intervals { get; set; } = new();
}