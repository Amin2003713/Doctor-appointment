using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.MedicalRecords.Requests;

public class GetEhrQuery : IRequest<PatientEhrResponse>
{
    public long PatientUserId { get; set; }
}