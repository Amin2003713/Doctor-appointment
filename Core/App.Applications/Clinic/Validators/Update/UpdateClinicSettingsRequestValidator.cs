using FluentValidation;

namespace App.Applications.Clinic.Validators.Update;

public class UpdateClinicSettingsRequestValidator : AbstractValidator<UpdateClinicSettingsRequest>
{
    public UpdateClinicSettingsRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام کلینیک الزامی است.")
            .MaximumLength(200)
            .WithMessage("نام کلینیک نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("آدرس کلینیک الزامی است.")
            .MaximumLength(500)
            .WithMessage("آدرس نمی‌تواند بیشتر از ۵۰۰ کاراکتر باشد.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره تلفن الزامی است.")
            .Matches(@"^(\+98|0)?9\d{9}$")
            .WithMessage("شماره تلفن معتبر نیست. (مثال: 09123456789)");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ایمیل الزامی است.")
            .EmailAddress()
            .WithMessage("ایمیل وارد شده معتبر نیست.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .When(x => x.Latitude.HasValue)
            .WithMessage("عرض جغرافیایی باید بین -۹۰ تا ۹۰ باشد.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .When(x => x.Longitude.HasValue)
            .WithMessage("طول جغرافیایی باید بین -۱۸۰ تا ۱۸۰ باشد.");

        RuleFor(x => x.DefaultVisitMinutes)
            .GreaterThan(0)
            .WithMessage("مدت زمان پیش‌فرض ویزیت باید بیشتر از صفر دقیقه باشد.")
            .LessThanOrEqualTo(240)
            .WithMessage("مدت زمان ویزیت نمی‌تواند بیشتر از ۴ ساعت باشد.");

        RuleFor(x => x.BufferBetweenVisitsMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("فاصله بین ویزیت‌ها نمی‌تواند منفی باشد.")
            .LessThanOrEqualTo(120)
            .WithMessage("فاصله بین ویزیت‌ها نمی‌تواند بیشتر از ۲ ساعت باشد.");
    }
}