using App.Applications.ClinicServices.Apis;
using App.Applications.ClinicServices.Requests.Get;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.ClinicServices.Get;

public class GetClinicServiceByIdHandler(
    ApiFactory       factory,
    ISnackbarService snackbar
) : IRequestHandler<GetClinicServiceByIdRequest, ClinicServiceResponse?>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task<ClinicServiceResponse?> Handle(GetClinicServiceByIdRequest request, CancellationToken ct)
    {
        var resp = await _api.GetById(request.Id, ct);

        if (resp is { IsSuccessStatusCode: true, Content: not null })
            return resp.Content;

        snackbar.ShowApiResult(resp.StatusCode);
        return null;
    }
}