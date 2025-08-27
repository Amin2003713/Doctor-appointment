using App.Applications.ClinicServices.Requests.Update;
using MediatR;

namespace App.Applications.ClinicServices.Requests.Create;

public class CreateClinicServiceRequest : UpsertServiceRequest,
    IRequest<Guid> { }