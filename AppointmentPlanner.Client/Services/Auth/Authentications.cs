using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;



#region Utilities

public static class JwtUtils
{
    private readonly static JwtSecurityTokenHandler Handler = new();

    public static ClaimsIdentity BuildIdentity(string jwt , string authenticationType = "jwt")
    {
        if (string.IsNullOrWhiteSpace(jwt)) return new ClaimsIdentity();

        var token  = Handler.ReadJwtToken(jwt);
        var claims = token.Claims.ToList();

        // Optionally map standard claims → ClaimTypes.Name etc.
        if (claims.Any(c => c.Type == ClaimTypes.Name))
            return new ClaimsIdentity(claims , authenticationType);

        var email = claims.FirstOrDefault(c => c.Type is "email" or ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email))
            claims.Add(new Claim(ClaimTypes.Name , email));

        return new ClaimsIdentity(claims , authenticationType);
    }

    public static bool IsExpired(DateTime expiresAtUtc , TimeSpan? skew = null)
    {
        var skewVal = skew ?? TimeSpan.FromMinutes(1); // default skew
        return DateTime.UtcNow.Add(skewVal) >= expiresAtUtc;
    }
}

#endregion

#region Storage

/// <summary>Single point of truth for auth state persisted in LocalStorage.</summary>
public interface ITokenStore
{
    ValueTask<AuthSession?> GetAsync(CancellationToken   ct                             = default);
    ValueTask               SetAsync(AuthSession         session , CancellationToken ct = default);
    ValueTask               ClearAsync(CancellationToken ct = default);
}

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

#endregion

#region API Client

/// <summary>Minimal, typed auth API boundary.</summary>
public interface IAuthApi
{
    Task<AuthTokenResponse>   LoginAsync(LoginRequest       request ,      CancellationToken ct);
    Task<AuthTokenResponse>   RefreshAsync(string           refreshToken , CancellationToken ct);
    Task<HttpResponseMessage> RegisterAsync(RegisterRequest request ,      CancellationToken ct);
}

public sealed class AuthApi(HttpClient http) : IAuthApi
{
    public async Task<AuthTokenResponse> LoginAsync(LoginRequest request , CancellationToken ct)
    {
        var resp = await http.PostAsJsonAsync("api/auth/login" , request , ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<AuthTokenResponse>(cancellationToken: ct)) ?? throw new InvalidOperationException("Empty auth response");
    }

    public async Task<AuthTokenResponse> RefreshAsync(string refreshToken , CancellationToken ct)
    {
        var resp = await http.PostAsJsonAsync("api/auth/refresh" ,
                                              new
                                              {
                                                  refreshToken
                                              } ,
                                              ct);

        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<AuthTokenResponse>(cancellationToken: ct)) ?? throw new InvalidOperationException("Empty refresh response");
    }

    public Task<HttpResponseMessage> RegisterAsync(RegisterRequest request , CancellationToken ct)
        => http.PostAsJsonAsync("api/auth/register" , request , ct);
}

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

/// <summary>
/// Injects Bearer token and auto-refreshes on expiry or 401 once per request.
/// </summary>
public sealed class AuthorizedHttpClientHandler(
    ITokenStore       tokens ,
    IAuthApi          authApi ,
    NavigationManager nav // used if you want to navigate on hard failure
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request , CancellationToken ct)
    {
        var session = await tokens.GetAsync(ct);

        // If we have a token and it's close to expiring, attempt proactive refresh.
        if (session is not null && JwtUtils.IsExpired(session.ExpiresAtUtc))
        {
            session = await TryRefreshAsync(session , ct);
        }

        if (session is not null)
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , session.AccessToken);
        }

        var response = await base.SendAsync(request , ct);

        // If unauthorized and we have a refresh token, try a single refresh, then replay the request.
        if (response.StatusCode != HttpStatusCode.Unauthorized || session is null)
            return response;

        response.Dispose(); // free the 401 response
        session = await TryRefreshAsync(session , ct);

        if (session is not null)
        {
            var clone = await CloneAsync(request , ct);

            clone.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , session.AccessToken);

            return await base.SendAsync(clone , ct);
        }

        // Optional: redirect to login on hard failure
        // nav.NavigateTo("/login", forceLoad: true);

        return response;
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

        // Copy headers
        foreach (var (key , values) in request.Headers)
            clone.Headers.TryAddWithoutValidation(key , values);

        // Copy content (buffered)
        if (request.Content is not null)
        {
            var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms , ct);
            ms.Position   = 0;
            clone.Content = new StreamContent(ms);

            foreach (var (key , values) in request.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(key , values);
        }

        // Copy options
        clone.Version       = request.Version;
        clone.VersionPolicy = request.VersionPolicy;

        return clone;
    }
}

#endregion

#region Auth Service (login / logout / register)

public interface IAuthService
{
    Task<bool> LoginAsync(string             email , string password , CancellationToken ct = default);
    Task       LogoutAsync(CancellationToken ct                                                               = default);
    Task<bool> RegisterAsync(string          email , string password , string fullName , CancellationToken ct = default);
}

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

#endregion

#region DI Wiring (Program.cs or equivalent)

// builder.Services.AddBlazoredLocalStorage();
public static class AuthDi
{
    /// <summary>Call this once during startup.</summary>
    public static IServiceCollection AddJwtAuth(this IServiceCollection services , Uri baseAddress)
    {
        services.AddScoped<ITokenStore , TokenStore>();
        services.AddScoped<ApiAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthStateProvider>());

        // Plain HttpClient for Auth endpoints (no bearer injection needed here)
        services.AddHttpClient<IAuthApi , AuthApi>(c => c.BaseAddress = baseAddress);

        // App HttpClient with Authorized handler
        services.AddTransient<AuthorizedHttpClientHandler>();
        services.AddHttpClient("App").ConfigureHttpClient(c => c.BaseAddress = baseAddress).AddHttpMessageHandler<AuthorizedHttpClientHandler>();

        services.AddScoped(sp =>
                               sp.GetRequiredService<IHttpClientFactory>().CreateClient("App"));

        services.AddScoped<IAuthService , AuthService>();

        return services;
    }
}

#endregion