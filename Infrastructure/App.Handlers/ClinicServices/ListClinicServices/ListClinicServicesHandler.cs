using App.Applications.ClinicServices;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

public class ListClinicServicesHandler(
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<ListClinicServicesRequest, List<ClinicServiceResponse>>
{
    private readonly IClinicServiceApis _api = factory.CreateApi<IClinicServiceApis>();

    public async Task<List<ClinicServiceResponse>> Handle(ListClinicServicesRequest request, CancellationToken ct)
    {
        var resp = await _api.List(ct);
        if (resp.IsSuccessStatusCode && resp.Content is not null)
            return resp.Content;

        snackbar.ShowApiResult(resp.StatusCode);
        return [];
    }
}