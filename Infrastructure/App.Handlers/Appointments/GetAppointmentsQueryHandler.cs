using App.Applications.Appointments.Apis;
using App.Applications.Appointments.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Refit;

namespace App.Handlers.Appointments;

public sealed class GetAppointmentsQueryHandler(
    ApiFactory                                   apiFactory,
    ISnackbarService                                     snackbar,
    IStringLocalizer<GetAppointmentsQueryHandler> localizer,
    ILogger<GetAppointmentsQueryHandler>          logger
) : IRequestHandler<GetAppointmentsQuery, List<AppointmentResponse>>
{
    public IAppointmentsApi api = apiFactory.CreateApi<IAppointmentsApi>();

    public async Task<List<AppointmentResponse>> Handle(GetAppointmentsQuery request, CancellationToken ct)
    {
        try
        {
            var list = await api.ListAsync(request.From, request.To, request.PatientUserId, ct);
            return list;
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex, "List appointments failed");
            snackbar.ShowError("List appointments failed");
            return new List<AppointmentResponse>();
        }
    }
}