// Application/Clinic/Dto.cs


using App.Domain.ValueObjects;

namespace App.Applications.Clinic.Requests.Get;

public class ClinicSettingsResponse
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Notes { get; set; }
    public PaymentMethods AcceptedPayments { get; set; }
    public int DefaultVisitMinutes { get; set; }
    public int BufferBetweenVisitsMinutes { get; set; }
}