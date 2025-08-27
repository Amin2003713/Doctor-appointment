using MediatR;

namespace App.Applications.ClinicServices.Requests.Update;

public class UpdateClinicServiceRequest : UpsertServiceRequest,
    IRequest
{
    public Guid Id { get; set; }
}