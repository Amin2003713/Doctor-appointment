using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class GetPrescriptionByIdRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionResponse>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<PrescriptionResponse> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.GetPrescriptionByIdAsync(request.Id, cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to retrieve prescription");
            Console.WriteLine(e);
            throw;
        }
    }
}