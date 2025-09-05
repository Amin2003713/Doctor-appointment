using App.Applications.DrugStore.Requests;
using App.Applications.DrugStore.Responses;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class UpsertDrugRequestValidator : AbstractValidator<UpsertDrugRequest>
{
    public UpsertDrugRequestValidator()
    {
        RuleFor(x => x.BrandName).NotEmpty().WithMessage("نام تجاری الزامی است.");

        RuleFor(x => x.GenericName).NotEmpty().WithMessage("نام عمومی الزامی است.");

        RuleFor(x => x.Form).Must(v => Enum.IsDefined(typeof(DrugForm), v)).WithMessage("شکل دارو نامعتبر است.");

        RuleFor(x => x.Route).Must(v => Enum.IsDefined(typeof(DrugRoute), v)).WithMessage("مسیر مصرف نامعتبر است.");

        RuleFor(x => x.RxClass).Must(v => Enum.IsDefined(typeof(RxClass), v)).WithMessage("کلاس دارو نامعتبر است.");

        RuleFor(x => x.StrengthValue).GreaterThanOrEqualTo(0).When(x => x.StrengthValue.HasValue).WithMessage("قدرت دارو نمی‌تواند منفی باشد.");
    }
}