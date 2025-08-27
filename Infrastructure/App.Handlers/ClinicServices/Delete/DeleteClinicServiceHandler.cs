using App.Applications.ClinicServices.Apis;
using App.Applications.ClinicServices.Requests.Delete;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.ClinicServices.Delete;

public class DeleteClinicServiceHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<DeleteClinicServiceRequest>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task Handle(DeleteClinicServiceRequest request, CancellationToken ct)
    {
        var resp = await _api.Delete(request.Id, ct);
        if (resp.IsSuccessStatusCode)
            snackbar.ShowSuccess("سرویس حذف شد.");
        else
            snackbar.ShowApiResult(resp.StatusCode);
    }
}