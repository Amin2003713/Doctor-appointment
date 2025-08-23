using FluentValidation;
using MediatR;

namespace App.Applications.Users.Requests;

public class ChangeRoleRequestValidator : AbstractValidator<ChangeRoleRequest>
{
    private static readonly string[] AllowedRoles =
    {
        "Doctor",
        "Secretary",
        "Patient"
    };

    public ChangeRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("شناسه کاربر الزامی است.")
            .Must(id => long.TryParse(id, out _))
            .WithMessage("شناسه کاربر باید عدد صحیح معتبر باشد.");

        RuleFor(x => x.NewRole)
            .NotEmpty()
            .WithMessage("انتخاب نقش جدید الزامی است.")
            .Must(role => AllowedRoles.Contains(role))
            .WithMessage($"نقش انتخابی معتبر نیست. نقش‌های مجاز عبارت‌اند از: {string.Join("، ", AllowedRoles)}");
    }
}