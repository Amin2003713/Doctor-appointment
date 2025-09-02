using MediatR;

namespace App.Applications.Schedules.Requests.GetSlots;

public record GetSlotsSummeryRequest(
    DateOnly Date,
    Guid     ServiceId,
    long patientUserId
) :
    IRequest<SlotsSummaryResponse>;