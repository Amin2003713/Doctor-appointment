#region

using App.Applications.Users.Requests.Verify;
using FluentValidation;

#endregion

namespace App.Applications.Users.Validators.SingUp;

public class SingUpEmployeeRequestValidator : AbstractValidator<VerifyPhoneNumberRequest>
{
    public SingUpEmployeeRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).
            NotEmpty().
            WithMessage("Phone number is required.").
            Matches(@"^09\d{9}$").
            WithMessage("Invalid phone number format. Use international format (e.g., 09131234567).");

        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.").Length(6).WithMessage("Code must be 6 digits.");
    }
}