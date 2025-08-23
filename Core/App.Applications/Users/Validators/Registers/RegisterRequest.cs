using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;

namespace App.Applications.Users.Requests.Registers.Patient;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    // الگوی رایج موبایل ایران: (+98|0098|0)?9xxxxxxxxx  یا حالت لوکال 9xxxxxxxxx
    private static readonly Regex IranMobileRegex =
        new(@"^(\+98|0098|0)?9\d{9}$", RegexOptions.Compiled);

    public RegisterRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره موبایل الزامی است.")
            .Must(IsValidIranMobile)
            .WithMessage("شماره موبایل نامعتبر است. مثال معتبر: 09123456789 یا +989123456789 یا 9123456789");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("رمز عبور الزامی است.")
            .MinimumLength(6)
            .WithMessage("رمز عبور باید حداقل ۶ کاراکتر باشد.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("نام الزامی است.")
            .MaximumLength(50)
            .WithMessage("حداکثر طول نام ۵۰ کاراکتر است.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("نام خانوادگی الزامی است.")
            .MaximumLength(80)
            .WithMessage("حداکثر طول نام خانوادگی ۸۰ کاراکتر است.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("نام و نام خانوادگی الزامی است.")
            .MaximumLength(130)
            .WithMessage("حداکثر طول نام کامل ۱۳۰ کاراکتر است.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("فرمت ایمیل صحیح نیست.");

        RuleFor(x => x.Address)
            .MaximumLength(300)
            .WithMessage("حداکثر طول آدرس ۳۰۰ کاراکتر است.");
    }

    private static bool IsValidIranMobile(string? value)
        => !string.IsNullOrWhiteSpace(value);
}