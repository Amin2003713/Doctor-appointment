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
        RuleFor(x => x.Form).InclusiveBetween(0, (int)Enum.GetValues(typeof(DrugForm)).Cast<DrugForm>().Max()).WithMessage("شکل دارو نامعتبر است.");
        RuleFor(x => x.Route).InclusiveBetween(0, (int)Enum.GetValues(typeof(DrugRoute)).Cast<DrugRoute>().Max()).WithMessage("مسیر مصرف نامعتبر است.");
        RuleFor(x => x.RxClass).InclusiveBetween(0, (int)Enum.GetValues(typeof(RxClass)).Cast<RxClass>().Max()).WithMessage("کلاس Rx نامعتبر است.");
        RuleFor(x => x.StrengthValue).GreaterThanOrEqualTo(0).When(x => x.StrengthValue.HasValue).WithMessage("قدرت دارو نمی‌تواند منفی باشد.");
    }
}