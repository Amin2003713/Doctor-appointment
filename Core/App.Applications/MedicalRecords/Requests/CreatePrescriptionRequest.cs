namespace App.Applications.MedicalRecords.Requests;

public class CreatePrescriptionRequest
{
    public Guid AppointmentId { get; set; }
    public long PatientUserId { get; set; }
    public string? Notes { get; set; }
    public int IssueMethod { get; set; }
    public List<PrescriptionItemDto> Items { get; set; } = new();
}