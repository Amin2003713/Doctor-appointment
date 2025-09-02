using App.Applications.MedicalRecords.CommandQueries;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validators;

public class GetMedicalRecordsByAppointmentQueryValidator : AbstractValidator<GetMedicalRecordsByAppointmentQuery>
{
    public GetMedicalRecordsByAppointmentQueryValidator()
    {
        RuleFor(x => x.AppointmentId).NotEqual(Guid.Empty);
        RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
            .WithMessage("Role must be Doctor, Secretary, or Patient.");
    }
}