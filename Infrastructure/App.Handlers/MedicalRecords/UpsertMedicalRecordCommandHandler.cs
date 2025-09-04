using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.Requests;
using App.Applications.MedicalRecords.Responses;

public class UpsertMedicalRecordCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<UpsertMedicalRecordCommand, MedicalRecordResponse>
{
    public IMedicalRecordsApi Apis { get; set; } = apiFactory.CreateApi<IMedicalRecordsApi>();

    public async Task<MedicalRecordResponse> Handle(UpsertMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Upsert(request.Body);

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