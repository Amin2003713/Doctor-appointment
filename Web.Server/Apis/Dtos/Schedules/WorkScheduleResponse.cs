

namespace Api.Endpoints.Dtos.Schedules;

public class WorkScheduleResponse
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}

public sealed class SlotsSummaryDto
{
    public List<string> StartTimes { get; set; } = new();       // ["09:00","09:45",...]
    public List<TimeRangeDto> Booked { get; set; } = new();     // [{from,to},...]
    public List<TimeRangeDto> FreeRanges { get; set; } = new(); // [{from,to},...]
}
