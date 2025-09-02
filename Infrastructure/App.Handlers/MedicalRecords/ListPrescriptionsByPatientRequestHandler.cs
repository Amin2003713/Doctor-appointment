using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class ListPrescriptionsByPatientRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<ListPrescriptionsByPatientQuery, List<PrescriptionResponse>>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<List<PrescriptionResponse>> Handle(ListPrescriptionsByPatientQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.ListPrescriptionsByPatientAsync(request.PatientUserId, cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to retrieve prescriptions");
            Console.WriteLine(e);
            throw;
        }
    }
}