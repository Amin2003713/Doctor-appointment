using App.Applications.Doctor.Requests.Get;
using MediatR;

namespace App.Applications.Doctor.Requests.Update;

public class UpsertDoctorProfileRequest : DoctorProfileResponse,
    IRequest
{
    public Guid Id { get; set; }
}