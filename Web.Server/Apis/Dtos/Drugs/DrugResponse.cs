namespace Api.Endpoints.Dtos.Drugs;

public sealed class DrugResponse
{
    public Guid Id { get; set; }
    public string BrandName { get; set; } = default!;
    public string GenericName { get; set; } = default!;
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
    public List<string> Synonyms { get; set; } = new();
    public bool IsActive { get; set; }
}