using App.Applications.MedicalRecords.CommandQueries;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class CancelPrescriptionCommandValidator : AbstractValidator<CancelPrescriptionCommand>
{
    public CancelPrescriptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor role.");
    }
}