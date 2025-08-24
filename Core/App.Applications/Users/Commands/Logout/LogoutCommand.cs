
using MediatR;

namespace App.Applications.Users.Commands.Logout;

public class LogoutCommand(string token) :  IRequest
{
    public string Token { get; set; } = token;
}