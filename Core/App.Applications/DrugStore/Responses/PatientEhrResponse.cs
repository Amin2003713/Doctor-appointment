using App.Applications.MedicalRecords.Responses;
using App.Applications.Prescriptions.Responses;

namespace App.Applications.DrugStore.Responses;

public class PatientEhrResponse
{
    public long PatientUserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public List<AppointmentSummary> Appointments { get; set; } = new();
    public List<MedicalRecordResponse> MedicalRecords { get; set; } = new();
    public List<PrescriptionResponse> Prescriptions { get; set; } = new();
}