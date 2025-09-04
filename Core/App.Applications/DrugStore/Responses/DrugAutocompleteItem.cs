namespace App.Applications.DrugStore.Responses;

public class DrugAutocompleteItem
{
    public Guid Id { get; set; }
    public string GenericName { get; set; } = string.Empty;
    public int Form { get; set; }
    public int Route { get; set; }
    public string Label { get; set; } = string.Empty;
}