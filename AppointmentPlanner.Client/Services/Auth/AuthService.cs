using AppointmentPlanner.Shared.AuthModels;
using Microsoft.AspNetCore.Components;

namespace AppointmentPlanner.Client.Services.Auth;

public sealed class AuthService(
    IAuthApi             api ,
    ITokenStore          tokens ,
    ApiAuthStateProvider stateProvider ,
    NavigationManager    nav
) : IAuthService
{
    public async Task<bool> LoginAsync(string email , string password , CancellationToken ct = default)
    {
        try
        {
            var res = await api.LoginAsync(new LoginRequest(email , password) , ct);

            var session = new AuthSession(
                res.AccessToken ,
                res.RefreshToken ,
                res.ExpiresAtUtc ,
                res.User
            );

            await tokens.SetAsync(session , ct);
            stateProvider.Notify(session);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        await tokens.ClearAsync(ct);
        stateProvider.Notify(null);
        nav.NavigateTo("/login" , forceLoad: true);
    }

    public async Task<bool> RegisterAsync(string email , string password , string fullName , CancellationToken ct = default)
    {
        var resp = await api.RegisterAsync(new RegisterRequest(email , password , fullName) , ct);
        return resp.IsSuccessStatusCode;
    }
}