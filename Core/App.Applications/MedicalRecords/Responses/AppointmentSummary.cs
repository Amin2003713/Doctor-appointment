using App.Applications.Appointments.Requests;
using App.Applications.ClinicServices.Requests.Get;

namespace App.Applications.MedicalRecords.Responses;

public class AppointmentSummary
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public ClinicServiceResponse Service { get; set; }
}