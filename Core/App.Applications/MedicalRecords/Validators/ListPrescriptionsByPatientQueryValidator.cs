using App.Applications.MedicalRecords.CommandQueries;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class ListPrescriptionsByPatientQueryValidator : AbstractValidator<ListPrescriptionsByPatientQuery>
{
    public ListPrescriptionsByPatientQueryValidator()
    {
        RuleFor(x => x.PatientUserId).GreaterThan(0);
        RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
            .WithMessage("Role must be Doctor, Secretary, or Patient.");
        When(x => x.Role == "Patient", () =>
        {
            RuleFor(x => x.UserId).NotNull().Equal(x => x.PatientUserId)
                .WithMessage("Patient can only access their own prescriptions.");
        });
    }
}