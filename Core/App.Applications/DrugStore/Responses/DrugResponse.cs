namespace App.Applications.DrugStore.Responses;

public class DrugResponse
{
    public Guid Id { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public DrugForm Form { get; set; }
    public DrugRoute Route { get; set; }
    public decimal? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
    public string? ConcentrationText { get; set; }
    public RxClass RxClass { get; set; }
    public string? Manufacturer { get; set; }
    public string? Country { get; set; }
    public string? Barcode { get; set; }
    public string? Tags { get; set; }
    public List<string> Synonyms { get; set; } = new();
    public bool IsActive { get; set; }
}