using MediatR;

namespace App.Applications.Prescriptions.Requests;

public class DispensePrescriptionCommand : IRequest
{
    public Guid Id { get; set; }
}