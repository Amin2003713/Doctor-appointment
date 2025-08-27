using App.Applications.ClinicServices.Requests.Get;
using MediatR;

namespace App.Applications.ClinicServices.Requests.ListClinicServices;

public record ListClinicServicesRequest() : IRequest<List<ClinicServiceResponse>>;