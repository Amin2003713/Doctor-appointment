using MediatR;

namespace App.Applications.ClinicServices;

public class UpdateClinicServiceRequest : UpsertServiceRequest,
    IRequest
{
    public Guid Id { get; set; }
}