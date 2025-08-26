namespace Api.Endpoints.Models.Clinic;

public class ClinicSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Basic info
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Notes { get; set; }

    // Map
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Payments accepted (enum flags)
    public PaymentMethods AcceptedPayments { get; set; } = PaymentMethods.Cash | PaymentMethods.Card;

    // Booking rules (global defaults)
    public int DefaultVisitMinutes { get; set; } = 20;
    public int BufferBetweenVisitsMinutes { get; set; } = 5;
}