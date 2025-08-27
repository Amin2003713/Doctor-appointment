// Handler

using App.Applications.ClinicServices;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

public class GetClinicServiceByIdHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<GetClinicServiceByIdRequest, ClinicServiceResponse?>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task<ClinicServiceResponse?> Handle(GetClinicServiceByIdRequest request, CancellationToken ct)
    {
        var resp = await _api.GetById(request.Id, ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null)
            return resp.Content;

        snackbar.ShowApiResult(resp.StatusCode);
        return null;
    }
}