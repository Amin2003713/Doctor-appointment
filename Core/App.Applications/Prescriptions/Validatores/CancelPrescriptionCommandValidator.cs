using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class CancelPrescriptionCommandValidator : AbstractValidator<CancelPrescriptionCommand>
{
    public CancelPrescriptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("شناسه نسخه الزامی است.");
    }
}