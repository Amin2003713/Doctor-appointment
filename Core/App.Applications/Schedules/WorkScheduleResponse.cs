// Application/Schedule/Dto.cs

namespace App.Applications.Schedules;

public class WorkScheduleResponse
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}