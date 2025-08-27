using App.Applications.Clinic.Requests.Get;
using App.Common.General;
using Refit;

namespace App.Applications.Clinic.Apis;

[Headers("Accept: application/json")]
public interface IClinicApis
{
    /// <summary>
    /// دریافت تنظیمات کلینیک
    /// GET api/clinic/settings
    /// </summary>
    [Get(ApiRoutes.Clinic.GetSettings)]
    Task<ApiResponse<ClinicSettingsResponse>> GetSettings(CancellationToken ct = default);

    /// <summary>
    /// به‌روزرسانی تنظیمات کلینیک (فقط نقش Doctor)
    /// PUT api/clinic/settings
    /// </summary>
    [Put(ApiRoutes.Clinic.UpdateSettings)]
    Task<ApiResponse<object>> UpdateSettings(
        [Body] UpdateClinicSettingsRequest body,
        CancellationToken ct = default);
}