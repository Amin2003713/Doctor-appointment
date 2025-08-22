using AppointmentPlanner.Shared.AuthModels;
using Blazored.LocalStorage;

namespace AppointmentPlanner.Client.Services.Auth;

public sealed class TokenStore(ILocalStorageService storage) : ITokenStore
{
    private const string Key = "auth_session_v1";

    public async ValueTask<AuthSession?> GetAsync(CancellationToken ct = default)
        => await storage.GetItemAsync<AuthSession>(Key , ct);

    public async ValueTask SetAsync(AuthSession session , CancellationToken ct = default)
        => await storage.SetItemAsync(Key , session , ct);

    public async ValueTask ClearAsync(CancellationToken ct = default)
        => await storage.RemoveItemAsync(Key , ct);
}