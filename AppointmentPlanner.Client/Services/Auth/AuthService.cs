using System.Net.Http.Json;
using AppointmentPlanner.Shared.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppointmentPlanner.Client.Services.Auth;

public class AuthService(
    HttpClient http,
    ILocalStorageService local,
    AuthenticationStateProvider provider,
    NavigationManager nav
)
{
    private readonly ApiAuthStateProvider _authState = (ApiAuthStateProvider)provider;

    public class AuthTokenResponse
    {
        public string AccessToken { get; set; } = "";
        public DateTime ExpiresAtUtc { get; set; }
        public MinimalUser User { get; set; } = new();
    }

    public class MinimalUser
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
    }

    public async Task<bool> LoginAsync(LoginDto dto)
    {
        var res = await http.PostAsJsonAsync("api/auth/login", dto);
        if (!res.IsSuccessStatusCode) return false;

        var payload = await res.Content.ReadFromJsonAsync<AuthTokenResponse>();
        if (payload is null) return false;

        await local.SetItemAsync("access_token",  payload.AccessToken);
        await local.SetItemAsync("user_email",    payload.User.Email);
        await local.SetItemAsync("user_name",     payload.User.FullName);
        await local.SetItemAsync("token_expires", payload.ExpiresAtUtc);

        await _authState.MarkUserAsAuthenticated(payload.User.Email);
        return true;
    }

    public async Task LogoutAsync()
    {
        await local.RemoveItemAsync("access_token");
        await local.RemoveItemAsync("user_email");
        await local.RemoveItemAsync("user_name");
        await local.RemoveItemAsync("token_expires");

        _authState.MarkUserAsLoggedOut();
        nav.NavigateTo("/login", forceLoad: true);
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var res = await http.PostAsJsonAsync("api/auth/register",
            dto);

        return res.IsSuccessStatusCode;
    }
}