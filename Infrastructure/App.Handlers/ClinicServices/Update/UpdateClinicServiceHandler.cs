using App.Applications.ClinicServices.Apis;
using App.Applications.ClinicServices.Requests.Update;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.ClinicServices.Update;

public class UpdateClinicServiceHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<UpdateClinicServiceRequest>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task Handle(UpdateClinicServiceRequest request, CancellationToken ct)
    {
        var resp = await _api.Update(request.Id, request, ct);
        if (resp.IsSuccessStatusCode)
            snackbar.ShowSuccess("سرویس با موفقیت بروزرسانی شد.");
        else
            snackbar.ShowApiResult(resp.StatusCode);
    }
}