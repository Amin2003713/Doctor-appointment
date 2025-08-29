using MediatR;

namespace App.Applications.Appointments.Requests;

public class RescheduleAppointmentCommand : UpsertAppointmentRequest,
                                            IRequest
{
    public Guid Id { get; set; }
}