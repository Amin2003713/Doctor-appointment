using App.Applications.Schedules.Apis;
using App.Applications.Schedules.Requests.Get;

namespace App.Handlers.Schedules;

public class GetWorkScheduleRequestHandler(
    ApiFactory apiFactory,
    ISnackbarService snackbar
)
    : IRequestHandler<GetWorkScheduleRequest, WorkScheduleResponse>
{
    private readonly IWorkScheduleApis _api = apiFactory.CreateApi<IWorkScheduleApis>();

    public async Task<WorkScheduleResponse> Handle(GetWorkScheduleRequest request, CancellationToken ct)
    {
        var resp = await _api.GetSchedule(ct);
        if (resp is { IsSuccessStatusCode: true, Content: not null })
            return resp.Content;

        snackbar.ShowError("امکان دریافت برنامه کاری وجود ندارد.");
        return new WorkScheduleResponse();
    }
}