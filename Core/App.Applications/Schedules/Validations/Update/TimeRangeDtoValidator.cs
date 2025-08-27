using App.Applications.Schedules.Requests.Get;
using FluentValidation;

namespace App.Applications.Schedules.Validations.Update;

public class TimeRangeDtoValidator : AbstractValidator<TimeRangeDto>
{
    public TimeRangeDtoValidator()
    {
        RuleFor(x => x.From)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("زمان شروع الزامی است.")
            .Must(TimeParsing.IsHhMm)
            .WithMessage("فرمت زمان شروع باید HH:mm باشد.");

        RuleFor(x => x.To)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("زمان پایان الزامی است.")
            .Must(TimeParsing.IsHhMm)
            .WithMessage("فرمت زمان پایان باید HH:mm باشد.");

        RuleFor(x => x)
            .Must(TimeParsing.FromLessThanTo)
            .WithMessage("زمان شروع باید کمتر از زمان پایان باشد.");
    }
}