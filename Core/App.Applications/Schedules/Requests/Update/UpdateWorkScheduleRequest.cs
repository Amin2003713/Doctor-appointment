using App.Applications.Schedules.Requests.Get;

namespace App.Applications.Schedules.Requests.Update;

public class UpdateWorkScheduleRequest
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}