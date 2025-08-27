using App.Applications.ClinicServices;
using App.Common.General;
using Refit;

public interface IClinicServiceApis
{
    [Get(ApiRoutes.Services.List)]
    Task<ApiResponse<List<ClinicServiceResponse>>> List(CancellationToken ct);

    [Get(ApiRoutes.Services.Get)]
    Task<ApiResponse<ClinicServiceResponse>> GetById(Guid id, CancellationToken ct);

    [Post(ApiRoutes.Services.Create)]
    Task<ApiResponse<Guid>> Create([Body] UpsertServiceRequest body, CancellationToken ct);

    [Put(ApiRoutes.Services.Update)]
    Task<ApiResponse<object>> Update(Guid id, [Body] UpsertServiceRequest body, CancellationToken ct);

    [Delete(ApiRoutes.Services.Delete)]
    Task<ApiResponse<object>> Delete(Guid id, CancellationToken ct);
}