using App.Applications.MedicalRecords.Responses;
using MediatR;

namespace App.Applications.MedicalRecords.Requests;

public class GetMedicalRecordsByAppointmentQuery : IRequest<List<MedicalRecordResponse>>
{
    public Guid AppointmentId { get; set; }
}