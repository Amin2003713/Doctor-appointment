public record RegisterOtherDto( // used by Doctor/Secretary endpoints
    string PhoneNumber,
    string Password,
    string FullName,
    string? Email
);