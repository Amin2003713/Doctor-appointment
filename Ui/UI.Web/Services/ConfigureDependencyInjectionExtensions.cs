using App.Applications.Users.Requests.UserInfos;
using App.Common.General;
using App.Common.Utilities.LifeTime;
using App.Domain.Users;
using App.Handlers.Users.Requests.Login;
using App.Persistence.Services.Refit;
using UI.Components.Features.Authorizions.Login;

namespace UI.Web.Services;

public static class ConfigureDependencyInjectionExtensions
{
    public static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        // Auto Register Services with Scrutor
        services.Scan(scan => scan.FromAssemblies(
                typeof(ApplicationConstants).Assembly ,
                typeof(UserInfo).Assembly ,
                typeof(UserInfoResponse).Assembly ,
                typeof(ApiFactory).Assembly ,
                typeof(LoginRequestHandler).Assembly ,
                typeof(Login).Assembly ,
                typeof(Program).Assembly
            )
            .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
            .AsImplementedInterfaces()
            .AsSelf()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
            .AsImplementedInterfaces()
            .AsSelf()
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
            .AsImplementedInterfaces()
            .AsSelf()
            .WithSingletonLifetime()
        );
    }
}