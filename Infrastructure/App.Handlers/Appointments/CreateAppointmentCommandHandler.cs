using App.Applications.Appointments.Apis;
using App.Applications.Appointments.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Refit;

namespace App.Handlers.Appointments;

public sealed class CreateAppointmentCommandHandler(
    ApiFactory                                        apiFactory, // your existing ApiFactory abstraction
    ISnackbarService                                  snackbar,
    IStringLocalizer<CreateAppointmentCommandHandler> localizer,
    ILogger<CreateAppointmentCommandHandler>          logger
) : IRequestHandler<CreateAppointmentCommand, Guid>
{
    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken ct)
    {
        try
        {
            var id = await api.CreateAsync(request, ct);
            snackbar.ShowSuccess(localizer["Appointment created"]);
            return id;
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Create appointment failed");
            snackbar.ShowError("error in api");
            return Guid.Empty;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Create appointment crashed");
            snackbar.ShowError("error in api");
            return Guid.Empty;
        }
    }
}