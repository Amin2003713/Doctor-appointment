using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class CreatePrescriptionRequestValidator : AbstractValidator<CreatePrescriptionRequest>
{
    public CreatePrescriptionRequestValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty().WithMessage("شناسه نوبت الزامی است.");
        RuleFor(x => x.PatientUserId).GreaterThan(0).WithMessage("شناسه بیمار الزامی است.");
        RuleFor(x => x.Items).NotEmpty().WithMessage("حداقل یک مورد الزامی است.");
        RuleForEach(x => x.Items).SetValidator(new CreatePrescriptionItemRequestValidator());
    }
}