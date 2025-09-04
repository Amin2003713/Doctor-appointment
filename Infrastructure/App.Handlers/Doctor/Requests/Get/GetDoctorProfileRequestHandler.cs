using App.Applications.Doctor.Apis;
using App.Applications.Doctor.Requests.Get;

namespace App.Handlers.Doctor.Requests.Get;

public class GetDoctorProfileRequestHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<GetDoctorProfileRequest, DoctorProfileResponse?>
{
    private readonly IDoctorApis _api = factory.CreateApi<IDoctorApis>();

    public async Task<DoctorProfileResponse?> Handle(GetDoctorProfileRequest request, CancellationToken ct)
    {
        var resp = await _api.GetProfile(ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null)
            return resp.Content;

        snackbar.ShowApiResult(resp.StatusCode);
        return null;
    }
}