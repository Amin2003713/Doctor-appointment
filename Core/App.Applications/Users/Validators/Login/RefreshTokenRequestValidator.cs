#region

using App.Applications.Users.Requests.Login;
using FluentValidation;

#endregion

namespace App.Applications.Users.Validators.Login;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

        RuleFor(x => x.AccessToken).NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
    }
}