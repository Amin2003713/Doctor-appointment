using MediatR;

namespace App.Applications.Users.Requests;

public class ChangeRoleRequest : IRequest
{
    public string UserId { get; set; }
    public string NewRole { get; set; }
}