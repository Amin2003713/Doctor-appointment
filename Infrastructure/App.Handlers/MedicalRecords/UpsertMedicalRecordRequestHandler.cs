using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class UpsertMedicalRecordRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<UpsertMedicalRecordCommand, MedicalRecordResponse>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<MedicalRecordResponse> Handle(UpsertMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.UpsertMedicalRecordAsync(request.Request, cancellationToken);
            snackbar.ShowSuccess("Medical record updated successfully");
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to update medical record");
            Console.WriteLine(e);
            throw;
        }
    }
}