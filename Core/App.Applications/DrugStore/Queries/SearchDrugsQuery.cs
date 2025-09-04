using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Queries;

public class SearchDrugsQuery : IRequest<DrugSearchResult>
{
    public string? Q { get; set; }
    public int? Form { get; set; }
    public int? Route { get; set; }
    public int? RxClass { get; set; }
    public bool? ActiveOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}