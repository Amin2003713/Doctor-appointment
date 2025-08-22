using App.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Domain.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddCommonServices();
        return services;
    }
}