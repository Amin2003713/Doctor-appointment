using App.Applications.DrugStore.Queries;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class AutocompleteDrugsQueryValidator : AbstractValidator<AutocompleteDrugsQuery>
{
    public AutocompleteDrugsQueryValidator()
    {
        RuleFor(x => x.Q).NotEmpty().WithMessage("عبارت جستجو الزامی است.");
        RuleFor(x => x.Limit).InclusiveBetween(1, 30).WithMessage("حداکثر نتایج باید بین ۱ تا ۳۰ باشد.");
    }
}