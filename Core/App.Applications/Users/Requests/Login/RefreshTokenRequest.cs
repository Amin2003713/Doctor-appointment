using App.Applications.Users.Response.Login;
using MediatR;

namespace App.Applications.Users.Requests.Login;

public record RefreshTokenRequest(
    string RefreshToken ,
    string AccessToken
) : IRequest<LoginResponse>;