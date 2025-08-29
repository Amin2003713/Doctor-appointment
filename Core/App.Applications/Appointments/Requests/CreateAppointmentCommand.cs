using MediatR;

namespace App.Applications.Appointments.Requests;

public class CreateAppointmentCommand : UpsertAppointmentRequest,
                                        IRequest<Guid>;