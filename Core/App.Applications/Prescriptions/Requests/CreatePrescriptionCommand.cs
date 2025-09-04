using App.Applications.Prescriptions.Responses;
using MediatR;

namespace App.Applications.Prescriptions.Requests;

public class CreatePrescriptionCommand : IRequest<PrescriptionResponse>
{
    public CreatePrescriptionRequest Body { get; set; } = new();
}