using App.Applications.ClinicServices.Requests.Get;
using App.Applications.ClinicServices.Requests.ListClinicServices;

namespace App.Handlers.ClinicServices.ListClinicServices;

public class ListClinicServicesHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<ListClinicServicesRequest, List<ClinicServiceResponse>>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task<List<ClinicServiceResponse>> Handle(ListClinicServicesRequest request, CancellationToken ct)
    {
        var resp = await _api.List(ct);
        if (resp is { IsSuccessStatusCode: true, Content: not null })
            return resp.Content;

        snackbar.ShowApiResult(resp.StatusCode);
        return [];
    }
}