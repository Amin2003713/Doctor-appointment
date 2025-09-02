using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class CreatePrescriptionRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<CreatePrescriptionCommand, PrescriptionResponse>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<PrescriptionResponse> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.CreatePrescriptionAsync(request.Request, cancellationToken);
            snackbar.ShowSuccess("Prescription created successfully");
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to create prescription");
            Console.WriteLine(e);
            throw;
        }
    }
}