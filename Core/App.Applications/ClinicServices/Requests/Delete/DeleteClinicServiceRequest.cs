using MediatR;

namespace App.Applications.ClinicServices.Requests.Delete;

public record DeleteClinicServiceRequest(
    Guid Id
) : IRequest;