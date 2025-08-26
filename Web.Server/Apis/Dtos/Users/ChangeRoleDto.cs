namespace Api.Endpoints.Dtos.Users;

public record ChangeRoleDto(
    string UserId, // string to accept long.ToString()
    string NewRole // "Patient" | "Secretary"
);