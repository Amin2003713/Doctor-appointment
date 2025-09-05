using App.Applications.DrugStore.Requests;
using App.Applications.DrugStore.Responses;
using Refit;

namespace App.Applications.DrugStore.Apis;

public interface IDrugsApi
{
    [Post("/api/drugs")]
    Task<ApiResponse<DrugResponse>> Create([Body] UpsertDrugRequest body);

    [Put("/api/drugs/{id}")]
    Task<ApiResponse<DrugResponse>> Update(Guid id, [Body] UpsertDrugRequest body);

    [Get("/api/drugs/{id}")]
    Task<ApiResponse<DrugResponse>> GetById(Guid id);

    [Get("/api/drugs/search")]
    Task<ApiResponse<DrugSearchResult>> Search(
        [Query] string? q,
        [Query] DrugForm? form,
        [Query] DrugRoute? route,
        [Query] RxClass? rxClass,
        [Query] bool? activeOnly,
        [Query] int page = 1,
        [Query] int pageSize = 20);


    [Get("/api/drugs/autocomplete")]
    Task<ApiResponse<List<DrugAutocompleteItem>>> Autocomplete([Query] string q, [Query] int limit = 10);

    [Get("/api/drugs/most-used")]
    Task<ApiResponse<List<DrugAutocompleteItem>>> MostUsed([Query] int days = 90, [Query] int limit = 15);
}