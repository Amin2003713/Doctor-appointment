using MediatR;

namespace App.Applications.Appointments.Requests;

public sealed record GetAppointmentsQuery(DateOnly? From, DateOnly? To, Guid? PatientUserId) : IRequest<List<AppointmentResponse>>;