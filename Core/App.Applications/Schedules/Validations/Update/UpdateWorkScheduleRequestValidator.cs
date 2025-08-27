using App.Applications.Schedules.Requests.Update;
using FluentValidation;

namespace App.Applications.Schedules.Validations.Update;

public class UpdateWorkScheduleRequestValidator : AbstractValidator<UpdateWorkScheduleRequest>
{
    public UpdateWorkScheduleRequestValidator()
    {
        RuleForEach(x => x.Days).SetValidator(new WorkingDayDtoValidator());
        RuleForEach(x => x.Overrides).SetValidator(new SpecialDateOverrideDtoValidator());

        RuleFor(x => x.Overrides)
            .Must(list => list is null || list.Select(o => o.Date).Distinct().Count() == list.Count)
            .WithMessage("برای یک تاریخ خاص فقط یک override مجاز است.");
    }
}