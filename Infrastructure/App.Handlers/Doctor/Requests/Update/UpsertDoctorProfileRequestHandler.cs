using App.Applications.Doctor.Apis;
using App.Applications.Doctor.Requests.Update;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Doctor.Requests.Update;

public class UpsertDoctorProfileRequestHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<UpsertDoctorProfileRequest>
{
    private readonly IDoctorApis _api = factory.CreateApi<IDoctorApis>();

    public async Task Handle(UpsertDoctorProfileRequest request, CancellationToken ct)
    {
        var resp = await _api.UpsertProfile(request, ct);

        if (resp.IsSuccessStatusCode)
            snackbar.ShowSuccess("پروفایل با موفقیت ذخیره شد.");
        else
            snackbar.ShowApiResult(resp.StatusCode);
    }
}