using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Queries;
using App.Applications.DrugStore.Responses;

public class GetDrugByIdQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<GetDrugByIdQuery, DrugResponse>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<DrugResponse> Handle(GetDrugByIdQuery request, CancellationToken cancellationToken)
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