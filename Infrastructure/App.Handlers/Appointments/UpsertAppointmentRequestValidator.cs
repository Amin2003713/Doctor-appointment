using App.Applications.Appointments.Requests;
using FluentValidation;

namespace App.Handlers.Appointments;

public sealed class UpsertAppointmentRequestValidator : AbstractValidator<UpsertAppointmentRequest>
{
    public UpsertAppointmentRequestValidator()
    {
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.Date).NotEqual(default(DateOnly));
        RuleFor(x => x.Start)
            .NotEmpty()
            .Matches("^(?:[01]\\d|2[0-3]):[0-5]\\d$") // HH:mm
            .WithMessage("Start must be HH:mm");
    }
}