using App.Applications.Prescriptions.Responses;

public class GetPrescriptionByIdQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionResponse>
{
    public IPrescriptionsApi Apis { get; set; } = apiFactory.CreateApi<IPrescriptionsApi>();

    public async Task<PrescriptionResponse> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.GetById(request.Id);

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