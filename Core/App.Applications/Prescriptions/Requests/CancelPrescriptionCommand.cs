using MediatR;

namespace App.Applications.Prescriptions.Requests;

public class CancelPrescriptionCommand : IRequest
{
    public Guid Id { get; set; }
}