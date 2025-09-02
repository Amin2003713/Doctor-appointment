namespace App.Applications.MedicalRecords.Requests;

public class PatientEhrResponse
{
    public long PatientUserId { get; set; }
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public List<AppointmentSummaryResponse> Appointments { get; set; } = new();
    public List<MedicalRecordResponse> MedicalRecords { get; set; } = new();
    public List<PrescriptionResponse> Prescriptions { get; set; } = new();
}