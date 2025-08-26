namespace Api.Endpoints.Models.Apointments;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ServiceId { get; set; }
    public Guid? PatientUserId { get; set; }                // when logged-in patient
    public string PatientFullName { get; set; } = default!; // quick entry by secretary/patient
    public string PatientPhone { get; set; } = default!;

    public DateOnly Date { get; set; }                      // appointment day
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }

    public decimal PriceAmount { get; set; }                // snapshot from service at booking time
    public string PriceCurrency { get; set; } = "ريال";

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;

    public string? Notes { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}