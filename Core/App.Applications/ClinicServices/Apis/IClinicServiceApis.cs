using App.Applications.ClinicServices.Requests.Create;
using App.Applications.ClinicServices.Requests.Get;
using App.Applications.ClinicServices.Requests.Update;
using App.Common.General;
using Refit;

namespace App.Applications.ClinicServices.Apis;

public interface IClinicServiceApis
{
    [Get(ApiRoutes.Services.List)]
    Task<ApiResponse<List<ClinicServiceResponse>>> List(CancellationToken ct);

    [Get(ApiRoutes.Services.Get)]
    Task<ApiResponse<ClinicServiceResponse>> GetById(Guid id, CancellationToken ct);

    [Post(ApiRoutes.Services.Create)]
    Task<ApiResponse<Guid>> Create([Body] CreateClinicServiceRequest body, CancellationToken ct);

    [Put(ApiRoutes.Services.Update)]
    Task<ApiResponse<object>> Update(Guid id, [Body] UpsertServiceRequest body, CancellationToken ct);

    [Delete(ApiRoutes.Services.Delete)]
    Task<ApiResponse<object>> Delete(Guid id, CancellationToken ct);
}