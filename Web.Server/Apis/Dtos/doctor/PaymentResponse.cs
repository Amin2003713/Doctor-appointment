using Api.Endpoints.Models.Clinic;

namespace Api.Endpoints.Dtos.doctor;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ريال";
    public DateTime PaidAt { get; set; }
    public PaymentMethods Method { get; set; }
    public string? TransactionReference { get; set; } // bank ref / gateway id
    public bool IsRefunded { get; set; }
    public bool Payed { get; set; } = false;
}