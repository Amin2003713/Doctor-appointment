using MediatR;

namespace App.Applications.Clinic.Requests.Get;

public record GetClinicSettingsInfo : IRequest<ClinicSettingsResponse>;