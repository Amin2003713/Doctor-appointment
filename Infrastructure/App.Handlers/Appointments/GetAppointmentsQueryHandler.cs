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
            var list = await api.ListAsync(request.From, request.To, request.PatientUserId);
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