using App.Applications.Appointments.Apis;
using App.Applications.Appointments.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Refit;

namespace App.Handlers.Appointments;

public sealed class CompleteAppointmentCommandHandler(
    ApiFactory                                          apiFactory,
    ISnackbarService                                    snackbar,
    IStringLocalizer<CompleteAppointmentCommandHandler> localizer,
    ILogger<CompleteAppointmentCommandHandler>          logger
) : IRequestHandler<CompleteAppointmentCommand>
{

    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task Handle(CompleteAppointmentCommand request, CancellationToken ct)
    {
        try
        {
            
            var res = await api.CompleteAsync(request.Id, ct);
            if (res.IsSuccessStatusCode)
            {
                snackbar.ShowSuccess(localizer["Marked as completed"]);
                return ;
            }
            snackbar.ShowError(localizer["Complete failed"]);
            return ;
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Complete appointment failed");
        }
    }
}