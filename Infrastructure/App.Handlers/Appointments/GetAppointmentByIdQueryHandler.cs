using App.Applications.Appointments.Apis;
using App.Applications.Appointments.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Refit;

namespace App.Handlers.Appointments;

public sealed class GetAppointmentByIdQueryHandler(
    ApiFactory                                       apiFactory,
    ISnackbarService                                 snackbar,
    IStringLocalizer<GetAppointmentByIdQueryHandler> localizer,
    ILogger<GetAppointmentByIdQueryHandler>          logger
) : IRequestHandler<GetAppointmentByIdQuery, AppointmentResponse?>
{
    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task<AppointmentResponse?> Handle(GetAppointmentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var item = await api.GetByIdAsync(request.Id, ct);
            return item;
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "Get-by-id failed");
            snackbar.ShowError("Get-by-id failed");
            return null;
        }
    }
}