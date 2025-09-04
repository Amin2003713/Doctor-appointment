using App.Applications.DrugStore.Commands;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class CreateDrugCommandValidator : AbstractValidator<CreateDrugCommand>
{
    public CreateDrugCommandValidator()
    {
        RuleFor(x => x.Body).SetValidator(new UpsertDrugRequestValidator());
    }
}