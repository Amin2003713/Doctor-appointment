using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.CommandQueries;
using App.Applications.MedicalRecords.Requests;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.MedicalRecords;

public class GetMedicalRecordsByAppointmentRequestHandler (
    ApiFactory factory,
    ISnackbarService snackbar
) : IRequestHandler<GetMedicalRecordsByAppointmentQuery, List<MedicalRecordResponse>>
{
    private readonly IMedicalRecordsApiClient _apis     = factory.CreateApi<IMedicalRecordsApiClient>();

    public async Task<List<MedicalRecordResponse>> Handle(GetMedicalRecordsByAppointmentQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var response = await _apis.GetMedicalRecordsByAppointmentAsync(request.AppointmentId, cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            snackbar.ShowError("Failed to retrieve medical records");
            Console.WriteLine(e);
            throw;
        }
    }
}