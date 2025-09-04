using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class CreatePrescriptionItemRequestValidator : AbstractValidator<CreatePrescriptionItemRequest>
{
    public CreatePrescriptionItemRequestValidator()
    {
        RuleFor(x => x.Dosage).NotEmpty().WithMessage("دوز الزامی است.");
        RuleFor(x => x.Frequency).NotEmpty().WithMessage("تکرار الزامی است.");
        RuleFor(x => x.Duration).NotEmpty().WithMessage("مدت زمان الزامی است.");
        RuleFor(x => x).Must(x => x.DrugId.HasValue || !string.IsNullOrWhiteSpace(x.DrugName)).WithMessage("یا شناسه دارو یا نام دارو الزامی است.");
    }
}