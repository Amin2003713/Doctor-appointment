using App.Applications.ClinicServices.Requests.Get;
using MediatR;

namespace App.Applications.ClinicServices.Requests.ClinicServiceById;

public record GetClinicServiceByIdRequest(
    Guid Id
) : IRequest<ClinicServiceResponse>;