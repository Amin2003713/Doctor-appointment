namespace Api.Endpoints.Models.Prescriptions;

public class PrescriptionItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrescriptionId { get; set; }
    public string DrugName { get; set; } = default!;
    public string? GenericName { get; set; }
    public string Dosage { get; set; } = default!;    // e.g., 500mg
    public string Frequency { get; set; } = default!; // e.g., q8h
    public string Duration { get; set; } = default!;  // e.g., 7 days
    public string? Instructions { get; set; }         // before meal...
    public bool IsPRN { get; set; }
    public int? RefillCount { get; set; }
}