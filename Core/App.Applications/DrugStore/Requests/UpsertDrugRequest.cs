// DTOs for Drugs

namespace App.Applications.DrugStore.Requests;

public class UpsertDrugRequest
{
    public string BrandName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public int Form { get; set; }
    public int Route { get; set; }
    public decimal? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
    public string? ConcentrationText { get; set; }
    public int RxClass { get; set; }
    public string? Manufacturer { get; set; }
    public string? Country { get; set; }
    public string? Barcode { get; set; }
    public string? Tags { get; set; }
    public List<string>? Synonyms { get; set; }
    public bool IsActive { get; set; }
}