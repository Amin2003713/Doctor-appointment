namespace App.Domain.ValueObjects;

public record Money(
    decimal Amount,
    string Currency = "ريال"
);