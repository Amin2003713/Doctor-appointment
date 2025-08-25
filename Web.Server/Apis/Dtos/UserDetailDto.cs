public record UserDetailDto(
    long Id,
    string PhoneOrUsername,
    string? Email,
    string? FirstName,
    string? LastName,
    string? FullName,
    string? Profile,
    string? Address,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc,
    IEnumerable<string> Roles
);


public sealed record UpdateProfileDto(
    string? FirstName,
    string? LastName,
    string? Address,
    string? Email  ,
    string? Profile
);