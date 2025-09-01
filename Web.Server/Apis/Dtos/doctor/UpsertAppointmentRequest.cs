namespace Api.Endpoints.Dtos.doctor;

public class UpsertAppointmentRequest
{
    public Guid ServiceId { get; set; }
    public DateOnly Date { get; set; }
    public string Start { get; set; } = default!; 
    public string? Notes { get; set; }

    public string? PatientFullName { get; set; }
    public string? PatientPhone { get; set; }
}