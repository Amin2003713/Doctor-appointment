using App.Common.General.BaseCommandQuery;
using MediatR;

namespace App.Applications.Users.Commands.Logout;

public class LogoutCommand(string token) : BaseCommandQuery , IRequest
{
    public string Token { get; set; } = token;
}