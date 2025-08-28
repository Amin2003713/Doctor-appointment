

namespace Api.Endpoints.Models.Schedule;

public class WorkSchedule 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<WorkingDay> Days { get; set; } = new();
    public List<SpecialDateOverride> Overrides { get; set; } = new();
}