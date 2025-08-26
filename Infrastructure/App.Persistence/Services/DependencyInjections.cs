#region

    using App.Applications.Services;
    using App.Persistence.Services.AuthState;
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.Extensions.DependencyInjection;

#endregion

    namespace App.Persistence.Services;

    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();

            services.AddScoped<ClientStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ClientStateProvider>());

            return services;
        }
    }