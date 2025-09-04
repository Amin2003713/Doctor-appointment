using App.Applications.MedicalRecords.Apis;
using App.Applications.MedicalRecords.Requests;
using App.Applications.MedicalRecords.Responses;

public class GetMedicalRecordsByAppointmentQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<GetMedicalRecordsByAppointmentQuery, List<MedicalRecordResponse>>
{
    public IMedicalRecordsApi Apis { get; set; } = apiFactory.CreateApi<IMedicalRecordsApi>();

    public async Task<List<MedicalRecordResponse>> Handle(GetMedicalRecordsByAppointmentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.ByAppointment(request.AppointmentId);

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