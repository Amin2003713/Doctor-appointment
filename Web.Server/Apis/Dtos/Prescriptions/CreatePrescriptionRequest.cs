namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class CreatePrescriptionRequest
{
    public Guid AppointmentId { get; set; }
    public Guid PatientUserId { get; set; }
    public string? Notes { get; set; }
    public int IssueMethod { get; set; } = 0; // Electronic
    public List<PrescriptionItemRequest> Items { get; set; } = new();
}