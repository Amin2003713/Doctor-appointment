using App.Applications.DrugStore.Queries;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class MostUsedDrugsQueryValidator : AbstractValidator<MostUsedDrugsQuery>
{
    public MostUsedDrugsQueryValidator()
    {
        RuleFor(x => x.Days).InclusiveBetween(1, 365).WithMessage("روزها باید بین ۱ تا ۳۶۵ باشد.");
        RuleFor(x => x.Limit).InclusiveBetween(1, 50).WithMessage("حداکثر نتایج باید بین ۱ تا ۵۰ باشد.");
    }
}