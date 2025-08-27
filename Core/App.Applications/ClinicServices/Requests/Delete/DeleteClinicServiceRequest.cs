using MediatR;

public record DeleteClinicServiceRequest(
    Guid Id
) : IRequest;