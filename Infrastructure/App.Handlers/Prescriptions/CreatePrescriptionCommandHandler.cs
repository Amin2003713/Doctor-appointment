using App.Applications.Prescriptions.Responses;

public class CreatePrescriptionCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<CreatePrescriptionCommand, PrescriptionResponse>
{
    public IPrescriptionsApi Apis { get; set; } = apiFactory.CreateApi<IPrescriptionsApi>();

    public async Task<PrescriptionResponse> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Create(request.Body);

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