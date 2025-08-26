using App.Applications.Users.Apis;
using App.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Applications.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLocalization(a => a.ResourcesPath = "Resources");
        var appAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName?.StartsWith("App.") == true)
            .ToArray();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(appAssemblies));
        services.AddDomainServices();
        services.AddValidatorsFromAssemblyContaining<IUserApis>();
        return services;
    }
}