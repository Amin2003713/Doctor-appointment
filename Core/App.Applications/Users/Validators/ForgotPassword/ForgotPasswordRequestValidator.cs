using App.Applications.Users.Requests.ForgotPassword;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace App.Applications.Users.Validators.ForgotPassword
{
    /// <summary>
    /// Validates the <see cref="ResetPasswordRequest"/> ensuring that required fields are present
    /// and follow the appropriate format rules.
    /// </summary>
    public class ForgotPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordRequestValidator"/> class.
        /// </summary>
        public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
        {
            // Phone number / UserName
            RuleFor(x => x.UserName).
                NotEmpty().
                WithMessage(localizer["PhoneNumberRequired"]).
                Matches(@"^\d{10,}$").
                WithMessage(localizer["PhoneNumberMinDigits"]);

            // New Password
            RuleFor(x => x.Password).
                NotEmpty().
                WithMessage(localizer["PasswordRequired"]).
                MinimumLength(6).
                WithMessage(localizer["PasswordMinLength"]).
                Matches(@"[A-Z]").
                WithMessage(localizer["PasswordUppercaseRequired"]).
                Matches(@"[a-z]").
                WithMessage(localizer["PasswordLowercaseRequired"]).
                Matches(@"\d").
                WithMessage(localizer["PasswordDigitRequired"]).
                Matches(@"[\W_]").
                WithMessage(localizer["PasswordSpecialRequired"]);

            // Confirm Password
            RuleFor(x => x.ConfirmPassword).
                NotEmpty().
                WithMessage(localizer["ConfirmPasswordRequired"]).
                Equal(x => x.Password).
                WithMessage(localizer["ConfirmPasswordMustMatch"]);
        }
    }
}

