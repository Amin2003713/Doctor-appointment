using App.Applications.MedicalRecords.CommandQueries;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class MarkPrescriptionDispensedCommandValidator : AbstractValidator<MarkPrescriptionDispensedCommand>
{
    public MarkPrescriptionDispensedCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}