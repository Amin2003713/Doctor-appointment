using App.Applications.Prescriptions.Responses;

public class ListPrescriptionsByPatientQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<ListPrescriptionsByPatientQuery, List<PrescriptionResponse>>
{
    public IPrescriptionsApi Apis { get; set; } = apiFactory.CreateApi<IPrescriptionsApi>();

    public async Task<List<PrescriptionResponse>> Handle(ListPrescriptionsByPatientQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.ListByPatient(request.PatientUserId);

            if (!result.IsSuccessStatusCode)
            {
                result.DeserializeAndThrow();
                return null!;
            }

            return result?.Content!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}