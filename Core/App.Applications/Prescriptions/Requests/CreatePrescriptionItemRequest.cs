namespace App.Applications.Prescriptions.Requests;

public class CreatePrescriptionItemRequest
{
    // Inferred from API code for body.Items
    public Guid? DrugId { get; set; }
    public string? DrugName { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int RefillCount { get; set; }
}