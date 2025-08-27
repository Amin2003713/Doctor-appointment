namespace App.Applications.Schedules;

public class UpdateWorkScheduleRequest
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}