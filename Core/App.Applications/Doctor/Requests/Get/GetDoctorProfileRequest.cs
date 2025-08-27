using MediatR;

namespace App.Applications.Doctor.Requests.Get;

public record GetDoctorProfileRequest() : IRequest<DoctorProfileResponse>;