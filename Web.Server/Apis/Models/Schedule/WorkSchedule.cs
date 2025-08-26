// Domain/Schedule/WorkSchedule.cs

namespace Api.Endpoints.Models.Schedule;

public class WorkSchedule // 1 row holds all weekly schedule + overrides
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<WorkingDay> Days { get; set; } = new();
    public List<SpecialDateOverride> Overrides { get; set; } = new();
}