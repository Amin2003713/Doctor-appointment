using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Queries;

public class GetDrugByIdQuery : IRequest<DrugResponse>
{
    public Guid Id { get; set; }
}