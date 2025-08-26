using App.Applications.Users.Requests.ChangeRoles;
using FluentValidation;

namespace App.Applications.Users.Validators.ChangeRoles;

public class ChangeRoleRequestValidator : AbstractValidator<ChangeRoleRequest>
{
    private readonly static string[] AllowedRoles =
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