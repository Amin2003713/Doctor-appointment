#region

using App.Applications.Users.Requests.SendActivationCode;
using FluentValidation;

#endregion

namespace App.Applications.Users.Validators.SendActivationCode;

public class SendActivationCodeValidator : AbstractValidator<SendActivationCodeRequest>
{
    public SendActivationCodeValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("شماره همراه الزامیست").Matches(@"^09\d{9}$").WithMessage("فرمت شماره همراه صحیح نمی‌باشد");
    }
}