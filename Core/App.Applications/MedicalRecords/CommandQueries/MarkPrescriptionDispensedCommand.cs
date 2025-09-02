using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record MarkPrescriptionDispensedCommand(
    Guid Id
) : IRequest;