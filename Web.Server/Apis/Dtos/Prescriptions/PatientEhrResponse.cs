namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class PatientEhrResponse
{
    public Guid PatientUserId { get; set; }
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    public List<AppointmentSummary> Appointments { get; set; } = new();
    public List<MedicalRecordResponse> MedicalRecords { get; set; } = new();
    public List<PrescriptionResponse> Prescriptions { get; set; } = new();
}