using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Applications.Users.Commands.Logout;
using App.Applications.Users.Queries.GetUserInfo;
using App.Common.Utilities.LifeTime;
using App.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace App.Persistence.Services.AuthState;

public class ClientStateProvider (
    IMediator mediator,
    NavigationManager navigationManager
) : AuthenticationStateProvider,
    IScopedDependency
{
    public UserInfo? User { get;  set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            User ??= await mediator.Send(new GetUserInfoQuery());


            if (User?.Token is null)
            {
                navigationManager.NavigateTo("/login");
                return Anonymous();
            }


            if (!IsTokenExpired(User.Token))
            {
                var principal = CreatePrincipal(User.Token);
                return new AuthenticationState(principal);
            }


            navigationManager.NavigateTo("/login");
            return Anonymous();
        }
        catch
        {
            navigationManager.NavigateTo("/login");
            return Anonymous();
        }
    }

    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private static bool IsTokenExpired(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        return jwt.ValidTo <= DateTime.UtcNow;
    }


    private static ClaimsPrincipal CreatePrincipal(string jwtToken)
    {
        var jwt      = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new ClaimsPrincipal(identity);
    }

    public async Task Logout()
    {
        if (User?.Token is not null)
        {
            try { await mediator.Send(new LogoutCommand(User.Token)); }
            catch
            {
                /* ignore */
            }
        }

        User = null;
        var anon = Task.FromResult(Anonymous());
        NotifyAuthenticationStateChanged(anon);
        navigationManager.NavigateTo("/login");
    }


    public Task SetUserAsync(UserInfo? user)
    {
        User = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}