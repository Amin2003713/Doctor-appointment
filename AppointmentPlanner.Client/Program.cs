using AppointmentPlanner.Shared.Models;
using AppointmentPlanner.Client.Pages;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

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

builder.Services.AddJwtAuth(new Uri(builder.HostEnvironment.BaseAddress));

await builder.Build().RunAsync();