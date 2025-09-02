using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class CancelPrescriptionRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<CancelPrescriptionCommand>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task Handle(CancelPrescriptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            await _apis.CancelPrescriptionAsync(request.Id, cancellationToken);
            snackbar.ShowSuccess("Prescription cancelled successfully");
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to cancel prescription");
            Console.WriteLine(e);
            throw;
        }
    }
}