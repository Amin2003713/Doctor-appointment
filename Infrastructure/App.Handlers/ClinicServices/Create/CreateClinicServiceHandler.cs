using App.Applications.ClinicServices.Apis;
using App.Applications.ClinicServices.Requests.Create;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.ClinicServices.Create;

public class CreateClinicServiceHandler(
    ApiFactory       factory,
    ISnackbarService snackbar
) : IRequestHandler<CreateClinicServiceRequest>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task Handle(CreateClinicServiceRequest request, CancellationToken ct)
    {
        var resp = await _api.Create(request, ct);

        if (resp.IsSuccessStatusCode)
        {
            snackbar.ShowSuccess("سرویس جدید ثبت شد.");
            return ;
        }

        snackbar.ShowApiResult(resp.StatusCode);
        return ;
    }
}