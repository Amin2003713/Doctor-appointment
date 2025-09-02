namespace Api.Endpoints.Models.Prescriptions;

public class MedicalRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Link back to appointment
    public Guid AppointmentId { get; set; }

    // Who created this record (Doctor or Secretary)
    public Guid? CreatedByUserId { get; set; }

    // Main free-text notes (required)
    public string Notes { get; set; } = default!;

    // Optional structured fields
    public string? Diagnosis { get; set; }

    public string? TreatmentPlan { get; set; }

    // Optional prescription text (separate from electronic prescription)
    public string? PrescriptionText { get; set; }

    // Any attachments (lab results, images, scans, etc.)
    public string? AttachmentsUrl { get; set; }

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}