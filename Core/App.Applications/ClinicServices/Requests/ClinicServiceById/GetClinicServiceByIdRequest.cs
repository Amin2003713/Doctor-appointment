using App.Applications.ClinicServices;
using MediatR;

public record GetClinicServiceByIdRequest(
    Guid Id
) : IRequest<ClinicServiceResponse>;