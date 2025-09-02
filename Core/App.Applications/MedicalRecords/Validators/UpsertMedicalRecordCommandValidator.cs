using App.Applications.MedicalRecords.CommandQueries;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class UpsertMedicalRecordCommandValidator : AbstractValidator<UpsertMedicalRecordCommand>
{
    public UpsertMedicalRecordCommandValidator()
    {
        RuleFor(x => x.Request.AppointmentId).NotEqual(Guid.Empty);
        RuleFor(x => x.Request.Notes).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.Request.Diagnosis).MaximumLength(1000).When(x => x.Request.Diagnosis != null);
        RuleFor(x => x.Request.PrescriptionText).MaximumLength(2000).When(x => x.Request.PrescriptionText != null);
        RuleFor(x => x.Request.AttachmentsUrl).MaximumLength(500).When(x => x.Request.AttachmentsUrl != null);
        RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary" }.Contains(r))
            .WithMessage("Role must be Doctor or Secretary.");
    }
}