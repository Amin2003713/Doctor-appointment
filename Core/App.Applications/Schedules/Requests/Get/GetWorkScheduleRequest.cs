using MediatR;

namespace App.Applications.Schedules.Requests.Get;

public record GetWorkScheduleRequest : IRequest<WorkScheduleResponse>;