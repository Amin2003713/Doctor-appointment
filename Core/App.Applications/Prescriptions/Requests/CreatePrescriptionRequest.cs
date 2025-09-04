namespace App.Applications.Prescriptions.Requests;

public class CreatePrescriptionRequest
{
    public Guid AppointmentId { get; set; }
    public long PatientUserId { get; set; }
    public int IssueMethod { get; set; }
    public string? Notes { get; set; }
    public List<CreatePrescriptionItemRequest> Items { get; set; } = new();
}