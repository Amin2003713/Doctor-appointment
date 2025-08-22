using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Applications.Users.Commands.Logout;
using App.Applications.Users.Queries.GetUserInfo;
using App.Applications.Users.Requests.Login;
using App.Common.Utilities.LifeTime;
using App.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace App.Persistence.Services.AuthState;

public class ClientStateProvider : AuthenticationStateProvider , IScopedDependency
{
    private readonly IMediator            _mediator;
    private readonly NavigationManager    _navigationManager;

    public ClientStateProvider(IMediator mediator ,NavigationManager navigationManager)
    {
        _mediator            = mediator;
        _navigationManager   = navigationManager;
        User                 = _mediator.Send(new GetUserInfoQuery()).Result;
    }

    public UserInfo? User { get; set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            User ??= await _mediator.Send(new GetUserInfoQuery());

            if (User?.Token == null || User.RefreshToken == null)
            {
                _navigationManager.NavigateTo("/login");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }


            if (!IsTokenExpired(User.Token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ExtractClaimsFromToken(User.Token) , "jwt")));

            if (IsTokenExpired(User?.Token!))
                return await RefreshTokenAsync(User);

            _navigationManager.NavigateTo("/login");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        catch (Exception)
        {
            _navigationManager.NavigateTo("/login");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private bool IsTokenExpired(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken   = jwtHandler.ReadJwtToken(token);
        return jwtToken.ValidTo < DateTime.Now;
    }

    private async Task<AuthenticationState> RefreshTokenAsync(UserInfo? user)
    {
        var login = await _mediator.Send(new RefreshTokenRequest(user!.RefreshToken , user.Token));
        User = user;
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ExtractClaimsFromToken(login.Token) , "jwt")));
    }

    private IEnumerable<Claim> ExtractClaimsFromToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken   = jwtHandler.ReadJwtToken(token);
        return jwtToken.Claims;
    }

    public async Task Logout()
    {
        if(User is { Token: not null })
            await _mediator.Send(new LogoutCommand(User.Token));

        var anonymousUser = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        User = null!;
        NotifyAuthenticationStateChanged(anonymousUser);
    }
}