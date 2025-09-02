using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record UpsertMedicalRecordCommand(
    UpsertMedicalRecordRequest Request,
    long? UserId,
    string Role
) : IRequest<MedicalRecordResponse>;