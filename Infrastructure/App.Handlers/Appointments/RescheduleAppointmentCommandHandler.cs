using App.Applications.Appointments.Apis;
using App.Applications.Appointments.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Refit;

namespace App.Handlers.Appointments;

public sealed class RescheduleAppointmentCommandHandler(
    ApiFactory                                            apiFactory,
    ISnackbarService                                      snackbar,
    IStringLocalizer<RescheduleAppointmentCommandHandler> localizer,
    ILogger<RescheduleAppointmentCommandHandler>          logger
) : IRequestHandler<RescheduleAppointmentCommand>
{
    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task Handle(RescheduleAppointmentCommand request, CancellationToken ct)
    {
        try
        {
            var res = await api.RescheduleAsync(request.Id, request, ct);

            if (res.IsSuccessStatusCode)
            {
                snackbar.ShowSuccess(localizer["Appointment rescheduled"]);
                return;
            }

            snackbar.ShowError(localizer["Reschedule failed"]);
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Reschedule appointment failed");
            snackbar.ShowError("Reschedule appointment failed");
        }
    }
}