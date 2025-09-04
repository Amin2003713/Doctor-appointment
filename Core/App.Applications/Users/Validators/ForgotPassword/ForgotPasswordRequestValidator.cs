using App.Applications.Users.Requests.ForgotPassword;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace App.Applications.Users.Validators.ForgotPassword;

public class ForgotPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(localizer["PhoneNumberRequired"]).Matches(@"^\d{10,}$").WithMessage(localizer["PhoneNumberMinDigits"]);


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(localizer["PasswordRequired"])
            .MinimumLength(6)
            .WithMessage(localizer["PasswordMinLength"])
            .Matches(@"[A-Z]")
            .WithMessage(localizer["PasswordUppercaseRequired"])
            .Matches(@"[a-z]")
            .WithMessage(localizer["PasswordLowercaseRequired"])
            .Matches(@"\d")
            .WithMessage(localizer["PasswordDigitRequired"])
            .Matches(@"[\W_]")
            .WithMessage(localizer["PasswordSpecialRequired"]);


        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(localizer["ConfirmPasswordRequired"])
            .Equal(x => x.Password)
            .WithMessage(localizer["ConfirmPasswordMustMatch"]);
    }
}