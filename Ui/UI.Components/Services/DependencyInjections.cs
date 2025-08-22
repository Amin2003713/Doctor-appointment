#region

using System.Globalization;
using App.Handlers.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

#endregion

namespace UI.Components.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddComponentServices(this IServiceCollection services)
    {
        services.AddMudServices(configuration =>
                                {
                                    configuration.ResizeOptions.EnableLogging         = true;
                                    configuration.ResizeObserverOptions.EnableLogging = true;


                                    configuration.SnackbarConfiguration.PositionClass          = Defaults.Classes.Position.TopLeft;
                                    configuration.SnackbarConfiguration.HideTransitionDuration = 200;
                                    configuration.SnackbarConfiguration.ShowTransitionDuration = 200;
                                    configuration.SnackbarConfiguration.VisibleStateDuration   = 5000;
                                    configuration.SnackbarConfiguration.ShowCloseIcon          = true;
                                    configuration.SnackbarConfiguration.PreventDuplicates      = true;
                                }).
                 AddMudBlazorSnackbar();

        services.AddLocalization(a => a.ResourcesPath = "Resources");


        services.AddHandlersServices();

        return services;
    }
}