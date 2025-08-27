using App.Applications.ClinicServices;
using MediatR;

public record ListClinicServicesRequest() : IRequest<List<ClinicServiceResponse>>;

