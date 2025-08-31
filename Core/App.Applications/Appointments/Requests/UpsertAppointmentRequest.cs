namespace App.Applications.Appointments.Requests;

public class UpsertAppointmentRequest
{
    public Guid? ServiceId { get; set; } = null!;
    public DateOnly? Date { get; set; }
    public string Start { get; set; } = default!;
    public string? Notes { get; set; }


    public string? PatientFullName { get; set; }
    public string? PatientPhone { get; set; }
}