namespace Api.Endpoints.Dtos.Drugs;

public sealed class DrugSearchResult
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<DrugResponse> Items { get; set; } = new();
}