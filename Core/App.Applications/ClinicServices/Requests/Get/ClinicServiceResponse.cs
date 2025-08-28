namespace App.Applications.ClinicServices.Requests.Get;

public class ClinicServiceResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "ريال";
    public int VisitMinutes { get; set; }
    public bool IsActive { get; set; }

    public bool IsEditing { get; set; } = false;
}