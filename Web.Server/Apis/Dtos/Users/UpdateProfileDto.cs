namespace Api.Endpoints.Dtos.Users;

public sealed record UpdateProfileDto(
    string? FirstName,
    string? LastName,
    string? Address,
    string? Email  ,
    string? Profile
);