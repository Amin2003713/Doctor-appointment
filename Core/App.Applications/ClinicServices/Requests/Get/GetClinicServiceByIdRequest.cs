// Application/Services/Dto.cs

using MediatR;

namespace App.Applications.ClinicServices;

public record GetClinicServiceByIdRequest() : IRequest<ClinicServiceResponse>;