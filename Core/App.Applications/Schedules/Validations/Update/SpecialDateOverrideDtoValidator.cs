using App.Applications.Schedules.Requests.Get;
using FluentValidation;

namespace App.Applications.Schedules.Validations.Update;

public class SpecialDateOverrideDtoValidator : AbstractValidator<SpecialDateOverrideDto>
{
    public SpecialDateOverrideDtoValidator()
    {
        RuleFor(x => x.Date).NotEmpty();

        When(x => !x.Closed,
            () =>
            {
                RuleFor(x => x.Intervals).NotEmpty().WithMessage("برای تاریخ‌های باز، حداقل یک بازه لازم است.");
                RuleForEach(x => x.Intervals).SetValidator(new TimeRangeDtoValidator());

                RuleFor(x => x.Intervals)
                    .Must(list => list != null && list.Count > 0 && !HasAnyOverlap(list))
                    .WithMessage("بین بازه‌های تاریخ خاص، همپوشانی وجود دارد.");
            });

        When(x => x.Closed,
            () =>
            {
                RuleFor(x => x.Intervals).Empty().WithMessage("برای تاریخ‌های بسته، بازه‌ای تعریف نکنید.");
            });
    }

    private static bool HasAnyOverlap(IReadOnlyList<TimeRangeDto> list)
    {
        if (list.Count <= 1) return false;

        for (var i = 0; i < list.Count; i++)
            for (var j = i + 1; j < list.Count; j++)
                if (TimeParsing.Overlaps(list[i], list[j]))
                    return true;

        return false;
    }
}