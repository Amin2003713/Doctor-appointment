using App.Applications.Schedules.Apis;
using App.Applications.Schedules.Requests.GetSlots;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Schedules;

public class GetSummerySlotsRequestHandler(
    ApiFactory       apiFactory,
    ISnackbarService snackbar
)
    : IRequestHandler<GetSlotsSummeryRequest, SlotsSummaryResponse>
{
    private readonly IWorkScheduleApis _api = apiFactory.CreateApi<IWorkScheduleApis>();

    public async Task<SlotsSummaryResponse> Handle(GetSlotsSummeryRequest request, CancellationToken ct)
    {
        var resp = await _api.GetSlotSummery(request.Date, request.ServiceId, request.patientUserId , ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null) return resp.Content;

        snackbar.ShowError("امکان دریافت اسلات‌های خالی وجود ندارد.");
        return new SlotsSummaryResponse();
    }
}