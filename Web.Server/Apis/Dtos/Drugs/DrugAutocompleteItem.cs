namespace Api.Endpoints.Dtos.Drugs;

public sealed class DrugAutocompleteItem
{
    public Guid Id { get; set; }
    public string Label { get; set; } = default!; // e.g., "Amoxil (Amoxicillin) 500 mg tablet"
    public string? GenericName { get; set; }
    public string? Strength { get; set; }
    public int Form { get; set; }
    public int Route { get; set; }
}