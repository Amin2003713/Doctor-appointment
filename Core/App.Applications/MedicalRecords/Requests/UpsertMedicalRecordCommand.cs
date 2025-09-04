using App.Applications.MedicalRecords.Responses;
using MediatR;

namespace App.Applications.MedicalRecords.Requests;

public class UpsertMedicalRecordCommand : IRequest<MedicalRecordResponse>
{
    public UpsertMedicalRecordRequest Body { get; set; } = new();
}