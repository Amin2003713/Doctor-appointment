namespace App.Applications.MedicalRecords.Requests;

public class UpsertMedicalRecordRequest
{
    public Guid AppointmentId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string? Diagnosis { get; set; }
    public string? PrescriptionText { get; set; }
    public string? AttachmentsUrl { get; set; }
}