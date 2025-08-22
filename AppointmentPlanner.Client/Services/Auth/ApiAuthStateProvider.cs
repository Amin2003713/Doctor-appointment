using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppointmentPlanner.Client.Services.Auth;

public class ApiAuthStateProvider (
    ILocalStorageService local
) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await local.GetItemAsStringAsync("access_token");
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        // You can parse JWT here to extract claims if you want
        var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, await local.GetItemAsStringAsync("user_email") ?? "")
            },
            "jwt");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public  Task MarkUserAsAuthenticated(string email)
    {
        var identity = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, email)
            ],
            "jwt");

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        return Task.CompletedTask;
    }

    public void MarkUserAsLoggedOut()
        => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
}