using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Common.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        // services.AddTransient(typeof(IPipelineBehavior<,>) , typeof(UnhandledExceptionBehaviour<,>));
        return services;
    }
}