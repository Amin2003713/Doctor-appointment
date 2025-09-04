using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Queries;
using App.Applications.DrugStore.Responses;

namespace App.Handlers.MedicalRecords;

public class AutocompleteDrugsQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<AutocompleteDrugsQuery, List<DrugAutocompleteItem>>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<List<DrugAutocompleteItem>> Handle(AutocompleteDrugsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Autocomplete(request.Q, request.Limit);

            if (result.IsSuccessStatusCode)
                return result?.Content!;

            result.DeserializeAndThrow();
            return null!;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}