using MediatR;

namespace App.Applications.Appointments.Requests;

public sealed record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentResponse?>;