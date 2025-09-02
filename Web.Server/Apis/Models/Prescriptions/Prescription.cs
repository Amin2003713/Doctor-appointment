namespace Api.Endpoints.Models.Prescriptions;

public class Prescription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; }
    public long PatientUserId { get; set; }
    public long PrescribedByUserId { get; set; }
    public DateTime IssuedAtUtc { get; set; } = DateTime.UtcNow;
    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Draft;
    public IssueMethod IssueMethod { get; set; } = IssueMethod.Electronic;
    public string? ErxCode { get; set; }
    public string? Notes { get; set; }

    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}