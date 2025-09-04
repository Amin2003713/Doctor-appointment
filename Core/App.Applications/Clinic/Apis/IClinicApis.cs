using App.Applications.Clinic.Requests.Get;
using App.Applications.Clinic.Requests.Update;
using App.Common.General;
using Refit;

namespace App.Applications.Clinic.Apis;

[Headers("Accept: application/json")]
public interface IClinicApis
{
    [Get(ApiRoutes.Clinic.GetSettings)]
    Task<ApiResponse<ClinicSettingsResponse>> GetSettings(CancellationToken ct = default);


    [Put(ApiRoutes.Clinic.UpdateSettings)]
    Task<ApiResponse<object>> UpdateSettings(
        [Body] UpdateClinicSettingsRequest body,
        CancellationToken ct = default);
}