using System.Security.Claims;
using AppointmentPlanner.Shared.AuthModels;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppointmentPlanner.Client.Services.Auth;

#region Utilities

#endregion

#region Storage

#endregion

#region API Client

#endregion

#region Auth State Provider

public sealed class ApiAuthStateProvider(ITokenStore tokens) : AuthenticationStateProvider
{
    private AuthenticationState _cached = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session = await tokens.GetAsync();

        if (session is null)
        {
            _cached = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            return _cached;
        }

        var identity = JwtUtils.BuildIdentity(session.AccessToken);
        _cached = new AuthenticationState(new ClaimsPrincipal(identity));
        return _cached;
    }

    public void Notify(AuthSession? session)
    {
        AuthenticationState state = session is null
            ? new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
            : new AuthenticationState(new ClaimsPrincipal(JwtUtils.BuildIdentity(session.AccessToken)));

        _cached = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }
}

#endregion

#region Delegating Handler (Bearer + Refresh)

#endregion

#region Auth Service (login / logout / register)

#endregion

