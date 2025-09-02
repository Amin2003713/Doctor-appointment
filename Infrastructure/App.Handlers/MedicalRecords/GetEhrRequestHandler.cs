using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class GetEhrRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<GetEhrQuery, PatientEhrResponse>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<PatientEhrResponse> Handle(GetEhrQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.GetEhrAsync(request.PatientUserId, cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to retrieve EHR");
            Console.WriteLine(e);
            throw;
        }
    }
}