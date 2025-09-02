namespace App.Applications.MedicalRecords.Requests;

public class UpsertMedicalRecordRequest
{
    public Guid AppointmentId { get; set; }
    public string Notes { get; set; } = null!;
    public string? Diagnosis { get; set; }
    public string? PrescriptionText { get; set; }
    public string? AttachmentsUrl { get; set; }
}