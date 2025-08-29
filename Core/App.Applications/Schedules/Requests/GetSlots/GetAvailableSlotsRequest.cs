using App.Applications.Schedules.Requests.Get;
using MediatR;

namespace App.Applications.Schedules.Requests.GetSlots;

public record GetAvailableSlotsRequest(
    DateOnly Date,
    Guid     ServiceId
) : IRequest<List<string>>;

public record GetSlotsSummeryRequest(
    DateOnly Date,
    Guid     ServiceId
) :
    IRequest<SlotsSummaryResponse>;

public sealed class SlotsSummaryResponse
{
    public List<string> StartTimes { get; set; } = new();       // ["09:00","09:45",...]
    public List<TimeRangeDto> Booked { get; set; } = new();     // [{from,to},...]
    public List<TimeRangeDto> FreeRanges { get; set; } = new(); // [{from,to},...]
}