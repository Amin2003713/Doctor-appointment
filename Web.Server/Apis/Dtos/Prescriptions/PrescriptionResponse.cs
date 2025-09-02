namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class PrescriptionResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public long PatientUserId { get; set; }
    public long PrescribedByUserId { get; set; }
    public DateTime IssuedAtUtc { get; set; }
    public int Status { get; set; }
    public int IssueMethod { get; set; }
    public string? ErxCode { get; set; }
    public string? Notes { get; set; }
    public List<PrescriptionItemDto> Items { get; set; } = new();
}