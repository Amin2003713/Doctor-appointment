namespace Api.Endpoints.Models.Apointments;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ServiceId { get; set; }
    public long? PatientUserId { get; set; }                
    public string PatientFullName { get; set; } = default!; 
    public string PatientPhone { get; set; } = default!;

    public DateOnly Date { get; set; }                      
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }

    public decimal PriceAmount { get; set; }                
    public string PriceCurrency { get; set; } = "ريال";

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;

    public string? Notes { get; set; }

    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}