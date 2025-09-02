using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record CreatePrescriptionCommand(
    CreatePrescriptionRequest Request,
    long? UserId
) : IRequest<PrescriptionResponse>;