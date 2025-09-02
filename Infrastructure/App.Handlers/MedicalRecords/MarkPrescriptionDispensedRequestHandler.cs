using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class MarkPrescriptionDispensedRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<MarkPrescriptionDispensedCommand>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task Handle(MarkPrescriptionDispensedCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            await _apis.MarkPrescriptionDispensedAsync(request.Id, cancellationToken);
            snackbar.ShowSuccess("Prescription marked as dispensed");
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to mark prescription as dispensed");
            Console.WriteLine(e);
            throw;
        }
    }
}