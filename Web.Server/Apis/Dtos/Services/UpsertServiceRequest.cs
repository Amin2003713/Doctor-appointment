namespace Api.Endpoints.Dtos.Services;

public class UpsertServiceRequest
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "ريال";
    public int VisitMinutes { get; set; } = 20;
    public bool IsActive { get; set; } = true;
}