using App.Applications.DrugStore.Queries;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class SearchDrugsQueryValidator : AbstractValidator<SearchDrugsQuery>
{
    public SearchDrugsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("شماره صفحه باید بزرگتر از ۰ باشد.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("اندازه صفحه باید بین ۱ تا ۱۰۰ باشد.");
    }
}