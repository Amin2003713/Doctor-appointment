using App.Applications.MedicalRecords.Requests;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validatores;

public class GetEhrQueryValidator : AbstractValidator<GetEhrQuery>
{
    public GetEhrQueryValidator()
    {
        RuleFor(x => x.PatientUserId).GreaterThan(0).WithMessage("شناسه بیمار الزامی است.");
    }
}