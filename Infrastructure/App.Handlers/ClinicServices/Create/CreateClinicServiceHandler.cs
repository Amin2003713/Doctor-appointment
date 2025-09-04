using App.Applications.ClinicServices.Requests.Create;

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
    }
}