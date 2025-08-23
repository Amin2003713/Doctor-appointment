public record RegisterDto(
    string PhoneNumber,
    string Password,
    string? Email,
    string? FirstName,
    string? LastName,
    string? Profile,
    string? Address
);