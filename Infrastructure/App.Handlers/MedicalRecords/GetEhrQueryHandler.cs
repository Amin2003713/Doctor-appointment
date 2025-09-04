using App.Applications.DrugStore.Responses;
using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.Requests;

public class GetEhrQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<GetEhrQuery, PatientEhrResponse>
{
    public IMedicalRecordsApi Apis { get; set; } = apiFactory.CreateApi<IMedicalRecordsApi>();

    public async Task<PatientEhrResponse> Handle(GetEhrQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.GetEhr(request.PatientUserId);

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