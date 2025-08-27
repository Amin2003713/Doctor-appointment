using App.Applications.Schedules.Requests.Get;
using FluentValidation;

namespace App.Applications.Schedules.Validations.Update;

public class WorkingDayDtoValidator : AbstractValidator<WorkingDayDto>
{
    public WorkingDayDtoValidator()
    {
        RuleFor(x => x.Day).IsInEnum();
        RuleForEach(x => x.Intervals).SetValidator(new TimeRangeDtoValidator());
        RuleForEach(x => x.Breaks).SetValidator(new TimeRangeDtoValidator());

        
        When(x => x.Closed,
            () =>
            {
                RuleFor(x => x.Intervals).Empty().WithMessage("برای روزهای بسته، بازه‌ای تعریف نکنید.");
                RuleFor(x => x.Breaks).Empty().WithMessage("برای روزهای بسته، وقفه‌ای تعریف نکنید.");
            });

        
        RuleFor(x => x.Intervals)
            .Must(list => !HasAnyOverlap(list))
            .WithMessage("بین بازه‌های کاری همپوشانی وجود دارد.");

        
        RuleFor(x => x.Breaks)
            .Must(list => !HasAnyOverlap(list))
            .WithMessage("بین بازه‌های وقفه همپوشانی وجود دارد.");

        
        RuleFor(x => x)
            .Must(BreaksAreWithinIntervals)
            .WithMessage("بازه‌های وقفه باید داخل بازه‌های کاری باشند.");
    }

    private static bool HasAnyOverlap(IReadOnlyList<TimeRangeDto>? list)
    {
        if (list is null || list.Count <= 1) return false;

        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
                if (TimeParsing.Overlaps(list[i], list[j]))
                    return true;

        return false;
    }

    private static bool BreaksAreWithinIntervals(WorkingDayDto d)
    {
        if (d.Breaks is null || d.Breaks.Count == 0) return true;
        if (d.Intervals is null || d.Intervals.Count == 0) return d.Breaks.Count == 0;

        bool Inside(TimeRangeDto inner, TimeRangeDto outer)
        {
            return TimeParsing.TryParse(inner.From, out var inf) &&
                   TimeParsing.TryParse(inner.To,   out var int_) &&
                   TimeParsing.TryParse(outer.From, out var of) &&
                   TimeParsing.TryParse(outer.To,   out var ot) &&
                   of <= inf &&
                   int_ <= ot;
        }

        foreach (var br in d.Breaks)
            if (!d.Intervals.Any(iv => Inside(br, iv)))
                return false;

        return true;
    }
}