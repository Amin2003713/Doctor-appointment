using App.Applications.Schedules.Apis;
using App.Applications.Schedules.Requests.Update;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using FluentValidation;
using MediatR;

namespace App.Handlers.Schedules;

public class UpdateWorkScheduleCommandHandler(
    ApiFactory apiFactory,
    ISnackbarService snackbar,
    IValidator<UpdateWorkScheduleRequest> validator
)
    : IRequestHandler<UpdateWorkScheduleCommand>
{
    private readonly IWorkScheduleApis _api = apiFactory.CreateApi<IWorkScheduleApis>();

    public async Task Handle(UpdateWorkScheduleCommand request, CancellationToken ct)
    {
        
        var v = await validator.ValidateAsync(request.Body, ct);

        if (!v.IsValid)
        {
            foreach (var err in v.Errors.DistinctBy(e => e.ErrorMessage))
                snackbar.ShowError(err.ErrorMessage);

            return;
        }

        var resp = await _api.UpdateSchedule(request.Body, ct);

        if (resp.IsSuccessStatusCode)
        {
            snackbar.ShowSuccess("برنامه کاری با موفقیت ذخیره شد.");
            return;
        }

        snackbar.ShowError("ذخیره برنامه کاری ناموفق بود.");
    }
}