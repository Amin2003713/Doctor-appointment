using MediatR;

namespace App.Applications.Users.Requests.ChangeRoles;

public record ChangeRoleRequest(
    string UserId,
    string NewRole
) : IRequest;