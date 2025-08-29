using App.Applications.Schedules.Requests.Get;

namespace App.Applications.Schedules.Requests.GetSlots;

public sealed class SlotsSummaryResponse
{
    public List<string> StartTimes { get; set; } = new();       // ["09:00","09:45",...]
    public List<TimeRangeDto> Booked { get; set; } = new();     // [{from,to},...]
    public List<TimeRangeDto> FreeRanges { get; set; } = new(); // [{from,to},...]
}