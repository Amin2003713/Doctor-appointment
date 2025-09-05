using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Queries;
using App.Applications.DrugStore.Responses;

public class SearchDrugsQueryHandler (
    ApiFactory apiFactory
) : IRequestHandler<SearchDrugsQuery, DrugSearchResult>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<DrugSearchResult> Handle(SearchDrugsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            var result = await Apis.Search(request.Q, request.Form.HasValue ? (int)request.Form.Value : null,
                                           request.Route.HasValue ? (int)request.Route.Value : null,
                                           request.RxClass.HasValue ? (int)request.RxClass.Value : null, request.ActiveOnly, request.Page, request.PageSize);

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