using Api.Endpoints.Models.Clinic;

namespace Api.Endpoints.Models.Payments;

public class Payment
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ريال";
    public DateTime PaidAt { get; set; }
    public PaymentMethods Method { get; set; }
    public string? TransactionReference { get; set; } // bank ref / gateway id
    public bool IsRefunded { get; set; }
}