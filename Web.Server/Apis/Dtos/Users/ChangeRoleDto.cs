namespace Api.Endpoints.Dtos.Users;

public record ChangeRoleDto(
    string UserId, 
    string NewRole 
);