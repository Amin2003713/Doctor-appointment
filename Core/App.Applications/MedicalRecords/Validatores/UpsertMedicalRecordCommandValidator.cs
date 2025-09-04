using App.Applications.MedicalRecords.Requests;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validatores;

public class UpsertMedicalRecordCommandValidator : AbstractValidator<UpsertMedicalRecordCommand>
{
    public UpsertMedicalRecordCommandValidator()
    {
        RuleFor(x => x.Body).SetValidator(new UpsertMedicalRecordRequestValidator());
    }
}