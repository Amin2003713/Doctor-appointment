namespace Api.Endpoints.Dtos.Schedules;

public sealed class SlotsSummaryDto
{
    public List<string> StartTimes { get; set; } = new();       // ["09:00","09:45",...]
    public List<TimeRangeDto> Booked { get; set; } = new();     // [{from,to},...]
    public List<TimeRangeDto> FreeRanges { get; set; } = new(); // [{from,to},...]
}