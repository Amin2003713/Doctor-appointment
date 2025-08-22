using System.Net;
using AppointmentPlanner.Shared.AuthModels;
using Microsoft.AspNetCore.Components;

namespace AppointmentPlanner.Client.Services.Auth;

/// <summary>
/// Injects Bearer token and auto-refreshes on expiry or 401 once per request.
/// </summary>
public sealed class AuthorizedHttpClientHandler(
    ITokenStore       tokens ,
    IAuthApi          authApi ,
    NavigationManager nav
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request , CancellationToken ct)
    {
        var session = await tokens.GetAsync(ct);


        if (session is not null && JwtUtils.IsExpired(session.ExpiresAtUtc))
            session = await TryRefreshAsync(session , ct);

        if (session is not null)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , session.AccessToken);

        var response = await base.SendAsync(request , ct);

        if (response.StatusCode != HttpStatusCode.Unauthorized || session is null)
            return response;

        response.Dispose();
        session = await TryRefreshAsync(session , ct);

        if (session is null)
            return response;

        var clone = await CloneAsync(request , ct);

        clone.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , session.AccessToken);

        return await base.SendAsync(clone , ct);
    }

    private async Task<AuthSession?> TryRefreshAsync(AuthSession current , CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(current.RefreshToken))
                return null;

            var refreshed = await authApi.RefreshAsync(current.RefreshToken , ct);

            var updated = new AuthSession(
                refreshed.AccessToken ,
                refreshed.RefreshToken ,
                refreshed.ExpiresAtUtc ,
                refreshed.User
            );

            await tokens.SetAsync(updated , ct);
            return updated;
        }
        catch
        {
            await tokens.ClearAsync(ct);
            return null;
        }
    }

    private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage request , CancellationToken ct)
    {
        var clone = new HttpRequestMessage(request.Method , request.RequestUri);

        foreach (var (key , values) in request.Headers)
            clone.Headers.TryAddWithoutValidation(key , values);


        if (request.Content is not null)
        {
            var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms , ct);
            ms.Position   = 0;
            clone.Content = new StreamContent(ms);

            foreach (var (key , values) in request.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(key , values);
        }

        clone.Version       = request.Version;
        clone.VersionPolicy = request.VersionPolicy;

        return clone;
    }
}