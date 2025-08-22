using AppointmentPlanner.Shared.AuthModels;

namespace AppointmentPlanner.Client.Services.Auth;

/// <summary>Single point of truth for auth state persisted in LocalStorage.</summary>
public interface ITokenStore
{
    ValueTask<AuthSession?> GetAsync(CancellationToken   ct                             = default);
    ValueTask               SetAsync(AuthSession         session , CancellationToken ct = default);
    ValueTask               ClearAsync(CancellationToken ct = default);
}