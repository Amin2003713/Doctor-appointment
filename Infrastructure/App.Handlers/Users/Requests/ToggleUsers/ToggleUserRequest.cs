using MediatR;

namespace App.Applications.Users.Requests.ToggleUsers;

public class ToggleUserRequest : IRequest<UserInfoResponse>
{
   public string UserId { get; set; } 
}