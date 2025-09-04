// Application/Services/Dto.cs

using MediatR;

namespace App.Applications.ClinicServices.Requests.Get;

public record GetClinicServiceByIdRequest(
    Guid Id
) : IRequest<ClinicServiceResponse>;