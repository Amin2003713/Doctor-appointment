using App.Applications.Users.Requests.UserQueries;
using FluentValidation;

namespace App.Applications.Users.Validators.UserQueries;

public class UsersQueryValidator : AbstractValidator<UsersQueryRequest>
{
    public UsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("شماره صفحه باید حداقل ۱ باشد.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("اندازه صفحه باید بین ۱ تا ۲۰۰ باشد.");

        When(x => !string.IsNullOrWhiteSpace(x.Search),
            () =>
            {
                RuleFor(x => x.Search!)
                    .MaximumLength(100)
                    .WithMessage("حداکثر طول عبارت جستجو ۱۰۰ کاراکتر است.");
            });
    }
}