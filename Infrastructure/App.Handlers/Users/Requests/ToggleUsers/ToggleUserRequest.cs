using App.Applications.Users.Requests.UserInfos;
using MediatR;

namespace App.Handlers.Users.Requests.ToggleUsers;

public class ToggleUserRequest : IRequest<UserInfoResponse>
{
   public string UserId { get; set; } 
}