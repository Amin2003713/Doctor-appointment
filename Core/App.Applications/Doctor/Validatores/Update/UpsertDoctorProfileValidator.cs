using App.Applications.Doctor.Requests.Update;
using FluentValidation;

namespace App.Applications.Doctor.Validatores.Update;

public class UpsertDoctorProfileValidator : AbstractValidator<UpsertDoctorProfileRequest>
{
    public UpsertDoctorProfileValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("نام کامل الزامی است.")
            .MaximumLength(100)
            .WithMessage("نام کامل نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Title)
            .MaximumLength(50)
            .WithMessage("عنوان شغلی نباید بیشتر از ۵۰ کاراکتر باشد.");

        RuleFor(x => x.Biography)
            .MaximumLength(1000)
            .WithMessage("بیوگرافی نباید بیشتر از ۱۰۰۰ کاراکتر باشد.");

        RuleForEach(x => x.Specialties)
            .MaximumLength(100)
            .WithMessage("هر تخصص نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleForEach(x => x.Education)
            .MaximumLength(100)
            .WithMessage("هر مورد تحصیلی نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleForEach(x => x.Languages)
            .MaximumLength(50)
            .WithMessage("نام زبان نباید بیشتر از ۵۰ کاراکتر باشد.");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .WithMessage("سال‌های تجربه نمی‌تواند منفی باشد.")
            .LessThanOrEqualTo(80)
            .WithMessage("سال‌های تجربه بیش از حد مجاز است.");

        RuleFor(x => x.PhotoUrl)
            .MaximumLength(300)
            .WithMessage("آدرس عکس نباید بیشتر از ۳۰۰ کاراکتر باشد.");

        RuleFor(x => x.Website)
            .MaximumLength(200)
            .WithMessage("آدرس وبسایت نباید بیشتر از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Instagram)
            .MaximumLength(100)
            .WithMessage("شناسه اینستاگرام نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.LinkedIn)
            .MaximumLength(100)
            .WithMessage("شناسه لینکدین نباید بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.WhatsApp)
            .MaximumLength(20)
            .WithMessage("شماره واتساپ نباید بیشتر از ۲۰ کاراکتر باشد.");
    }
}