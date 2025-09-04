namespace App.Applications.DrugStore.Responses;

public class DrugSearchResult
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<DrugResponse> Items { get; set; } = new();
}