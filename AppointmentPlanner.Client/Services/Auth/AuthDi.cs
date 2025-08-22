using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;

namespace AppointmentPlanner.Client.Services.Auth;

public static class AuthDi
{
    /// <summary>Call this once during startup.</summary>
    public static IServiceCollection AddClientJwtAuth(this IServiceCollection services , Uri baseAddress)
    {
        services.AddRefitClient<IAuthApi>().ConfigureHttpClient(c => c.BaseAddress = baseAddress);

        services.AddScoped<ITokenStore , TokenStore>();
        services.AddScoped<ApiAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthStateProvider>());
        services.AddScoped<IAuthService , AuthService>();
        services.AddBlazoredLocalStorageAsSingleton();
        // Refit-based Auth API

        // Authorized app client
        services.AddTransient<AuthorizedHttpClientHandler>();
        services.AddHttpClient("App").ConfigureHttpClient(c => c.BaseAddress = baseAddress).AddHttpMessageHandler<AuthorizedHttpClientHandler>();

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("App"));


        return services;
    }
}