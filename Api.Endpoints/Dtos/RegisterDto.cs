public record RegisterDto(
    string PhoneNumber, // required (username)
    string Password,    // required
    string FullName,    // required
    string? Email       // optional
);