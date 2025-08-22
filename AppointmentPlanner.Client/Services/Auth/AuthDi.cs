using Microsoft.AspNetCore.Components.Authorization;
using Refit;

namespace AppointmentPlanner.Client.Services.Auth;

public static class AuthDi
{
    /// <summary>Call this once during startup.</summary>
    public static IServiceCollection AddClientJwtAuth(this IServiceCollection services , Uri baseAddress)
    {
        services.AddScoped<ITokenStore , TokenStore>();
        services.AddScoped<ApiAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthStateProvider>());

        // Refit-based Auth API
        services.AddRefitClient<IAuthApi>().ConfigureHttpClient(c => c.BaseAddress = baseAddress);

        // Authorized app client
        services.AddTransient<AuthorizedHttpClientHandler>();
        services.AddHttpClient("App").ConfigureHttpClient(c => c.BaseAddress = baseAddress).AddHttpMessageHandler<AuthorizedHttpClientHandler>();

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("App"));

        services.AddScoped<IAuthService , AuthService>();

        return services;
    }
}