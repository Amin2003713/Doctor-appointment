namespace App.Applications.Schedules.Requests.Get;

public class WorkScheduleResponse
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}