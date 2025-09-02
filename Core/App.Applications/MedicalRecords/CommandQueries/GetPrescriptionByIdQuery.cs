using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record GetPrescriptionByIdQuery(
    Guid Id,
    long? UserId,
    string Role
) : IRequest<PrescriptionResponse>;