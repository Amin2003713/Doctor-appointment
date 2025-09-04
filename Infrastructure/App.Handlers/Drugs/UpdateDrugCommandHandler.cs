using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Commands;
using App.Applications.DrugStore.Responses;

public class UpdateDrugCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<UpdateDrugCommand, DrugResponse>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<DrugResponse> Handle(UpdateDrugCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Update(request.Id, request.Body);

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