using App.Applications.Users.Queries.GetUserInfo;
using App.Handlers.Users.Queries.GetUserInfo;
using App.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Handlers.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddHandlersServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
                            {
                                cfg.RegisterServicesFromAssemblies(typeof(GetUserInfoQueryHandler).Assembly);
                            });

        services.AddInfrastructureServices();
        return services;
    }
}