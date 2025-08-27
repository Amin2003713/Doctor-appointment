namespace App.Applications.doctor;

public class UpsertAppointmentRequest
{
    public Guid ServiceId { get; set; }
    public DateOnly Date { get; set; }
    public string Start { get; set; } = default!; // "HH:mm"
    public string? Notes { get; set; }

    // Patient fields: required if creator is Secretary/Doctor (booking for someone)
    public string? PatientFullName { get; set; }
    public string? PatientPhone { get; set; }
}