using AppointmentPlanner.Shared.Models;
using AppointmentPlanner.Client.Pages;
using AppointmentPlanner.Client.Services.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddClientJwtAuth(new Uri(builder.HostEnvironment.BaseAddress));


builder.Services.AddMudServices(config =>
                                {
                                    config.SnackbarConfiguration.PositionClass          = Defaults.Classes.Position.BottomRight;
                                    config.SnackbarConfiguration.PreventDuplicates      = false;
                                    config.SnackbarConfiguration.NewestOnTop            = false;
                                    config.SnackbarConfiguration.ShowCloseIcon          = true;
                                    config.SnackbarConfiguration.VisibleStateDuration   = 5000;
                                    config.SnackbarConfiguration.HideTransitionDuration = 500;
                                    config.SnackbarConfiguration.ShowTransitionDuration = 500;
                                    config.SnackbarConfiguration.SnackbarVariant        = Variant.Filled;
                                });


await builder.Build().RunAsync();