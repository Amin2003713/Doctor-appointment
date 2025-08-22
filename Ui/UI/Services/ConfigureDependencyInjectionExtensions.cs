using App.Common.General;
using App.Common.Utilities.LifeTime;
using App.Components.Services.AssetManager;
using App.Domain.Users;

using App.Persistence.Services.Refit;

namespace App.Services;

public static class ConfigureDependencyInjectionExtensions
{
    public static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        // Auto Register Services with Scrutor
        services.Scan(scan => scan.FromAssemblies(
                                       typeof(ApplicationConstants).Assembly ,
                                       typeof(UserInfo).Assembly ,
                                       typeof(ApiFactory).Assembly ,
                                       typeof(AssetManager).Assembly ,
                                       typeof(Program).Assembly
                                   ).
                                   AddClasses(classes => classes.AssignableTo<IScopedDependency>()).
                                   AsImplementedInterfaces().
                                   AsSelf().
                                   WithScopedLifetime().
                                   AddClasses(classes => classes.AssignableTo<ITransientDependency>()).
                                   AsImplementedInterfaces().
                                   AsSelf().
                                   WithTransientLifetime().
                                   AddClasses(classes => classes.AssignableTo<ISingletonDependency>()).
                                   AsImplementedInterfaces().
                                   AsSelf().
                                   WithSingletonLifetime()
        );
    }
}