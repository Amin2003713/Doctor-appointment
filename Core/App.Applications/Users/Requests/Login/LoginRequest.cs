using App.Applications.Users.Response.Login;
using MediatR;
using MudBlazor;

namespace App.Applications.Users.Requests.Login;

public class LoginRequest : IRequest<LoginResponse>
{
    [Label("نام کاربری")] public string PhoneNumber { get; set; }

    [Label("کلمه عبور")] public string Password { get; set; }

    public string? Refresh_token { get; set; } = null;
}