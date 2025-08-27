using App.Applications.ClinicServices;
using MediatR;

public class CreateClinicServiceRequest : UpsertServiceRequest,
    IRequest<Guid> { }
