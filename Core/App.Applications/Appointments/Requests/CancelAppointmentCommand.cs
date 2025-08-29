using MediatR;

namespace App.Applications.Appointments.Requests;

public record CancelAppointmentCommand(Guid Id) : IRequest;