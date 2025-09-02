using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
{
    public CreatePrescriptionCommandValidator()
    {
        RuleFor(x => x.Request.AppointmentId).NotEqual(Guid.Empty);
        RuleFor(x => x.Request.PatientUserId).GreaterThan(0);
        RuleFor(x => x.Request.Items).NotEmpty();
        RuleForEach(x => x.Request.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.DrugName).NotEmpty().MaximumLength(100);
            item.RuleFor(i => i.GenericName).MaximumLength(100).When(i => i.GenericName != null);
            item.RuleFor(i => i.Dosage).NotEmpty().MaximumLength(50);
            item.RuleFor(i => i.Frequency).NotEmpty().MaximumLength(50);
            item.RuleFor(i => i.Duration).NotEmpty().MaximumLength(50);
            item.RuleFor(i => i.Instructions).MaximumLength(500).When(i => i.Instructions != null);
            item.RuleFor(i => i.RefillCount).GreaterThanOrEqualTo(0);
        });
        RuleFor(x => x.Request.IssueMethod).Must(im => Enum.IsDefined(typeof(IssueMethod), im));
        RuleFor(x => x.Request.Notes).MaximumLength(2000).When(x => x.Request.Notes != null);
        RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor role.");
    }
}