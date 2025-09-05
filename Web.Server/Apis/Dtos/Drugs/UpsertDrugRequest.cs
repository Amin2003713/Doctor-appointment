using Api.Endpoints.Models.Drugs;

namespace Api.Endpoints.Dtos.Drugs;

public sealed class UpsertDrugRequest
{
    public string BrandName { get; set; } = default!;
    public string GenericName { get; set; } = default!;
    public DrugForm Form { get; set; }  // DrugForm
    public DrugRoute Route { get; set; } // DrugRoute
    public decimal? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
    public string? ConcentrationText { get; set; }
    public RxClass RxClass { get; set; } = RxClass.Rx;          // Rx by default
    public string? Manufacturer { get; set; }
    public string? Country { get; set; }
    public string? Barcode { get; set; }
    public string? Tags { get; set; }              // comma-separated
    public List<string>? Synonyms { get; set; }
    public bool IsActive { get; set; } = true;
}