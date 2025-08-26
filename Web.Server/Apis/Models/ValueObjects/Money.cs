// Common/ValueObjects.cs

using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Models.ValueObjects;

[Owned]
public record Money(
    decimal Amount,
    string Currency = "ريال"
);