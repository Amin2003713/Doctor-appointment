using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record CancelPrescriptionCommand(
    Guid Id,
    long? UserId
) : IRequest;