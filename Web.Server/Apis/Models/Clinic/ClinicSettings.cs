namespace Api.Endpoints.Models.Clinic;

public class ClinicSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();

    
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Notes { get; set; }

    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    
    public PaymentMethods AcceptedPayments { get; set; } = PaymentMethods.Cash | PaymentMethods.Card;

    
    public int DefaultVisitMinutes { get; set; } = 20;
    public int BufferBetweenVisitsMinutes { get; set; } = 5;
}