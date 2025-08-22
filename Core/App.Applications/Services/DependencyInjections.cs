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

        services.AddDomainServices();
        services.AddValidatorsFromAssemblyContaining<IUserApis>();
        return services;
    }
}