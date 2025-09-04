using App.Applications.DrugStore.Commands;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class UpdateDrugCommandValidator : AbstractValidator<UpdateDrugCommand>
{
    public UpdateDrugCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("شناسه دارو الزامی است.");
        RuleFor(x => x.Body).SetValidator(new UpsertDrugRequestValidator());
    }
}