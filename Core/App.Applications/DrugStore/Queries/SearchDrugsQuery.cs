using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Queries;

public class SearchDrugsQuery : IRequest<DrugSearchResult>
{
    public string? Q { get; set; }
    public DrugForm? Form { get; set; }
    public DrugRoute? Route { get; set; }
    public RxClass? RxClass { get; set; }
    public bool? ActiveOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}