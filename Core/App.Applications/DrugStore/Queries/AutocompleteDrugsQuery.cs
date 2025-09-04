using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Queries;

public class AutocompleteDrugsQuery : IRequest<List<DrugAutocompleteItem>>
{
    public string Q { get; set; } = string.Empty;
    public int Limit { get; set; } = 10;
}