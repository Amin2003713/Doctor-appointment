public record UserListItemDto(
    long Id,
    string PhoneNumber,
    string? Email,
    string? FullName,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc,
    IEnumerable<string> Roles
);