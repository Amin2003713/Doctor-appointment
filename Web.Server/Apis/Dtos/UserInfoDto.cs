public record UserInfoDto(
    long Id,
    string PhoneOrUsername,
    string? Email,
    string? FirstName,
    string? LastName,
    string? FullName,
    string? Profile,
    string? Address,
    string? PrimaryRole
);