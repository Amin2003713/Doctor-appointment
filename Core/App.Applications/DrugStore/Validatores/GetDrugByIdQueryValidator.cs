using App.Applications.DrugStore.Queries;
using FluentValidation;

namespace App.Applications.DrugStore.Validatores;

public class GetDrugByIdQueryValidator : AbstractValidator<GetDrugByIdQuery>
{
    public GetDrugByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("شناسه دارو الزامی است.");
    }
}