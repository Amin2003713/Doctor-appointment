using App.Applications.MedicalRecords.Requests;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validatores;

public class UpsertMedicalRecordRequestValidator : AbstractValidator<UpsertMedicalRecordRequest>
{
    public UpsertMedicalRecordRequestValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty().WithMessage("شناسه نوبت الزامی است.");
        RuleFor(x => x.Notes).NotEmpty().WithMessage("یادداشت‌ها الزامی است.");
    }
}