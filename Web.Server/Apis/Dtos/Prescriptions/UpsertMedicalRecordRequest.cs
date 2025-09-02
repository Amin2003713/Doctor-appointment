// MedicalRecord

namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class UpsertMedicalRecordRequest
{
    public Guid AppointmentId { get; set; }
    public string Notes { get; set; } = default!;
    public string? Diagnosis { get; set; }
    public string? PrescriptionText { get; set; } // متن آزاد
    public string? AttachmentsUrl { get; set; }
}

// Prescription