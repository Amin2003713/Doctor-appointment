public class CancelPrescriptionCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<CancelPrescriptionCommand>
{
    public IPrescriptionsApi Apis { get; set; } = apiFactory.CreateApi<IPrescriptionsApi>();

    public async Task Handle(CancelPrescriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Cancel(request.Id);

            if (!result.IsSuccessStatusCode)
            {
                result.DeserializeAndThrow();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}