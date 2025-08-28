namespace Api.Endpoints.Models.MediaclService;

public class MedicalRecord
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string Notes { get; set; } = default!;
    public string? Diagnosis { get; set; }
    public string? Prescription { get; set; }
    public string? AttachmentsUrl { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}