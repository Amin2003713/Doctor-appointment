using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record GetEhrQuery(
    long PatientUserId,
    long? UserId,
    string Role
) : IRequest<PatientEhrResponse>;