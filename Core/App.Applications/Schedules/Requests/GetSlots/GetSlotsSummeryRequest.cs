using MediatR;

namespace App.Applications.Schedules.Requests.GetSlots;

public record GetSlotsSummeryRequest(
    DateOnly Date,
    Guid     ServiceId
) :
    IRequest<SlotsSummaryResponse>;