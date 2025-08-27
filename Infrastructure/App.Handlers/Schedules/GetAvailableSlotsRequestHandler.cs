using App.Applications.Schedules.Apis;
using App.Applications.Schedules.Requests.GetSlots;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Schedules;

public class GetAvailableSlotsRequestHandler(
    ApiFactory apiFactory,
    ISnackbarService snackbar
)
    : IRequestHandler<GetAvailableSlotsRequest, List<string>>
{
    private readonly IWorkScheduleApis _api = apiFactory.CreateApi<IWorkScheduleApis>();

    public async Task<List<string>> Handle(GetAvailableSlotsRequest request, CancellationToken ct)
    {
        var resp = await _api.GetSlots(request.Date, request.ServiceId, ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null) return resp.Content;

        snackbar.ShowError("امکان دریافت اسلات‌های خالی وجود ندارد.");
        return new();
    }
}