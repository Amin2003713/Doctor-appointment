using MediatR;

namespace App.Applications.Schedules.Requests.Update;

public record UpdateWorkScheduleCommand(
    UpdateWorkScheduleRequest Body
) : IRequest;