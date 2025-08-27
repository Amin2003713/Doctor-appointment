using MediatR;

namespace App.Applications.Schedules.Requests.GetSlots;

public record GetAvailableSlotsRequest(
    DateOnly Date,
    Guid ServiceId
) : IRequest<List<string>>;
