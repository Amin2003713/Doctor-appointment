using App.Applications.Prescriptions.Responses;
using MediatR;

namespace App.Applications.Prescriptions.Requests;

public class ListPrescriptionsByPatientQuery : IRequest<List<PrescriptionResponse>>
{
    public long PatientUserId { get; set; }
}