namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class PrescriptionItemRequest
{
    public Guid? DrugId { get; set; }     // optional; when set, we resolve names from Drug
    public string? DrugName { get; set; } // optional fallback when DrugId is null
    public string Dosage { get; set; } = default!;
    public string Frequency { get; set; } = default!;
    public string Duration { get; set; } = default!;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int? RefillCount { get; set; }
}