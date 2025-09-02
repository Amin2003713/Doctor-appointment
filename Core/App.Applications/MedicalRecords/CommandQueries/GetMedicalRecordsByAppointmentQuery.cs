using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record GetMedicalRecordsByAppointmentQuery(
    Guid AppointmentId,
    long? UserId,
    string Role
) : IRequest<List<MedicalRecordResponse>>;