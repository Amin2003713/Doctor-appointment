namespace App.Applications.Prescriptions.Responses;

public class PrescriptionItemDto
{
    public Guid Id { get; set; }
    public Guid? DrugId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string? GenericName { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int RefillCount { get; set; }
}