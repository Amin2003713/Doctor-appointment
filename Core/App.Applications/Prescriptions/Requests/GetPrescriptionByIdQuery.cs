using App.Applications.Prescriptions.Responses;
using MediatR;

namespace App.Applications.Prescriptions.Requests;

public class GetPrescriptionByIdQuery : IRequest<PrescriptionResponse>
{
    public Guid Id { get; set; }
}