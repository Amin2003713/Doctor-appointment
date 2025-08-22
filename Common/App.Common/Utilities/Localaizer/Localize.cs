using Microsoft.Extensions.DependencyInjection;

namespace App.Common.Utilities.Localaizer;

public static class Localize
{
    public static IServiceCollection AddResources(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        return services;
    }
}