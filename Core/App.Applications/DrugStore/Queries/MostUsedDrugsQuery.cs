using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Queries;

public class MostUsedDrugsQuery : IRequest<List<DrugAutocompleteItem>>
{
    public int Days { get; set; } = 90;
    public int Limit { get; set; } = 15;
}