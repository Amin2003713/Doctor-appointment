using MediatR;

namespace App.Applications.Appointments.Requests;

public record CompleteAppointmentCommand(Guid Id) : IRequest;