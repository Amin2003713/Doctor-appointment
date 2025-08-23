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
            .WithMessage("گذرواژه الزامی است.")
            .MinimumLength(6)
            .WithMessage("حداقل طول گذرواژه ۶ کاراکتر است.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("نام و نام خانوادگی الزامی است.")
            .MinimumLength(2)
            .WithMessage("نام و نام خانوادگی حداقل باید ۲ کاراکتر باشد.")
            .MaximumLength(128)
            .WithMessage("نام و نام خانوادگی حداکثر می‌تواند ۱۲۸ کاراکتر باشد.");

        When(x => !string.IsNullOrWhiteSpace(x.Email),
            () =>
            {
                RuleFor(x => x.Email!)
                    .EmailAddress()
                    .WithMessage("ایمیل نامعتبر است.");
            });
    }

    private static bool IsValidIranMobile(string? value)
        => !string.IsNullOrWhiteSpace(value) && IranMobileRegex.IsMatch(value.Trim());
}