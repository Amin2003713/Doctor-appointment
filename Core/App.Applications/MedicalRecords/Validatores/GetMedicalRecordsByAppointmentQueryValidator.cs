using App.Applications.MedicalRecords.Requests;
using FluentValidation;

namespace App.Applications.MedicalRecords.Validatores;

public class GetMedicalRecordsByAppointmentQueryValidator : AbstractValidator<GetMedicalRecordsByAppointmentQuery>
{
    public GetMedicalRecordsByAppointmentQueryValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty().WithMessage("شناسه نوبت الزامی است.");
    }
}