using App.Applications.Schedules.Apis;
using App.Applications.Schedules.Requests.GetSlots;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Schedules;

public class GetAvailableSlotsRequestHandler(
    ApiFactory       apiFactory,
    ISnackbarService snackbar
)
    : IRequestHandler<GetAvailableSlotsRequest, Dictionary<string, bool>>
{
    private readonly IWorkScheduleApis _api = apiFactory.CreateApi<IWorkScheduleApis>();

    public async Task<Dictionary<string, bool>> Handle(GetAvailableSlotsRequest request, CancellationToken ct)
    {
        var resp = await _api.GetSlotSummery(request.Date, request.ServiceId, request.patientUserId, ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null) return ConvertToSlotsAvailability(resp.Content);

        snackbar.ShowError("امکان دریافت اسلات‌های خالی وجود ندارد.");
        return new Dictionary<string, bool>();
    }

    private static Dictionary<string, bool> ConvertToSlotsAvailability(SlotsSummaryResponse summary)
    {
        var slots = new Dictionary<string, bool>();

        // Initialize all start times as free (false)
        foreach (var startTime in summary.StartTimes)
        {
            slots[startTime] = false;
        }

        // Mark booked slots as true
        foreach (var bookedRange in summary.Booked)
        {
            if (slots.ContainsKey(bookedRange.From))
            {
                slots[bookedRange.From] = true;
            }
        }

        return slots;
    }
}