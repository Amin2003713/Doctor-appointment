using MediatR;

namespace App.Applications.Users.Requests.ChangeRoles;

public class ChangeRoleRequest : IRequest
{
    public string UserId { get; set; }
    public string NewRole { get; set; }
}