using App.Applications.Prescriptions.Requests;
using FluentValidation;

namespace App.Applications.Prescriptions.Validatores;

public class ListPrescriptionsByPatientQueryValidator : AbstractValidator<ListPrescriptionsByPatientQuery>
{
    public ListPrescriptionsByPatientQueryValidator()
    {
        RuleFor(x => x.PatientUserId).GreaterThan(0).WithMessage("شناسه بیمار الزامی است.");
    }
}