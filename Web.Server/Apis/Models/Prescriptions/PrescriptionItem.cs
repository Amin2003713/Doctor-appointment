namespace Api.Endpoints.Models.Prescriptions;

using System.ComponentModel.DataAnnotations;


public class PrescriptionItem
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public Guid PrescriptionId { get; set; }

    public Guid? DrugId { get; set; }

    [Required, MaxLength(200)] public string DrugName { get; set; } = default!;

    [Required, MaxLength(64)] public string Dosage { get; set; } = default!;

    [Required, MaxLength(64)] public string Frequency { get; set; } = default!;

    [Required, MaxLength(64)] public string Duration { get; set; } = default!;

    [MaxLength(500)] public string? Instructions { get; set; }

    public bool IsPRN { get; set; }

    public int? RefillCount { get; set; }
}
