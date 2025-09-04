using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
{
    public CreatePrescriptionCommandValidator()
    {
        RuleFor(x => x.Body).SetValidator(new CreatePrescriptionRequestValidator());
    }
}