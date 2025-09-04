public class DispensePrescriptionCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<DispensePrescriptionCommand>
{
    public IPrescriptionsApi Apis { get; set; } = apiFactory.CreateApi<IPrescriptionsApi>();

    public async Task Handle(DispensePrescriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Dispense(request.Id);

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