using MediatR;

namespace App.Applications.Schedules.Requests.GetSlots;

public record GetAvailableSlotsRequest(
    DateOnly Date,
    Guid     ServiceId,
    long     patientUserId
) : IRequest<List<string>>;