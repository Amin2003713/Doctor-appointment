using MudBlazor;

namespace App.Handlers.Appointments;

public sealed class CancelAppointmentCommandHandler(
    ApiFactory                                        apiFactory,
    ISnackbar                                         snackbar,
    IStringLocalizer<CancelAppointmentCommandHandler> localizer,
    ILogger<CancelAppointmentCommandHandler>          logger
) : IRequestHandler<CancelAppointmentCommand>
{
    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task Handle(CancelAppointmentCommand request, CancellationToken ct)
    {
        try
        {
            var res = await api.CancelAsync(request.Id, ct);

            if (res.IsSuccessStatusCode)
            {
                snackbar.Add(localizer["Appointment canceled"], Severity.Info);
                return;
            }

            snackbar.Add(localizer["Cancel failed"], Severity.Error);
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Cancel appointment failed");
        }
    }
}