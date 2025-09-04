using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Queries;
using App.Applications.DrugStore.Responses;

public class MostUsedDrugsQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<MostUsedDrugsQuery, List<DrugAutocompleteItem>>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<List<DrugAutocompleteItem>> Handle(MostUsedDrugsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.MostUsed(request.Days, request.Limit);

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