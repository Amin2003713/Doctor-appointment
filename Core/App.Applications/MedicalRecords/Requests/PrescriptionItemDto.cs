namespace App.Applications.MedicalRecords.Requests;

public class PrescriptionItemDto
{
    public Guid Id { get; set; }
    public string DrugName { get; set; } = null!;
    public string? GenericName { get; set; }
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int RefillCount { get; set; }
}