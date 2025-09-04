using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class GetPrescriptionByIdQueryValidator : AbstractValidator<GetPrescriptionByIdQuery>
{
    public GetPrescriptionByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("شناسه نسخه الزامی است.");
    }
}