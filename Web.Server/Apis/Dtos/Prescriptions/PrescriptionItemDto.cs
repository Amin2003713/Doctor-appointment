namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class PrescriptionItemDto
{
    public Guid Id { get; set; }
    public string DrugName { get; set; } = default!;
    public string? GenericName { get; set; }
    public string Dosage { get; set; } = default!;
    public string Frequency { get; set; } = default!;
    public string Duration { get; set; } = default!;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int? RefillCount { get; set; }
}