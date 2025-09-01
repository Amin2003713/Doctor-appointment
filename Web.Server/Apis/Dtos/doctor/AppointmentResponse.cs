using Api.Endpoints.Models.Apointments;

namespace Api.Endpoints.Dtos.doctor;

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceTitle { get; set; } = default!;
    public DateOnly Date { get; set; }
    public string Start { get; set; } = default!;
    public string End { get; set; } = default!;
    public AppointmentStatus Status { get; set; }
    public string PatientFullName { get; set; } = default!;
    public string PatientPhone { get; set; } = default!;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "ريال";
    public string? Notes { get; set; }

    public long PatientId { get; set; } = default!;

}