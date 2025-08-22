using App.Applications.Users.Requests.Login;
using FluentValidation;

namespace App.Applications.Users.Validators.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("نام کاربری اجباری است.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور اجباری است.").MinimumLength(8).WithMessage("طول رمز عبور باید حداقل 8 کاراکتر باشد");
    }
}