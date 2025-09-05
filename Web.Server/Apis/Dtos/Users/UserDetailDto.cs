namespace Api.Endpoints.Dtos.Users;

public record UserDetailDto(
    long Id,
    string Username,
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