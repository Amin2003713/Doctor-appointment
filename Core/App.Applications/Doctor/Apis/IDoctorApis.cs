using App.Applications.Doctor.Requests.Get;
using App.Applications.Doctor.Requests.Update;
using App.Common.General;
using Refit;

namespace App.Applications.Doctor.Apis;

public interface IDoctorApis
{
    [Get(ApiRoutes.Doctor.GetProfile)]
    Task<ApiResponse<DoctorProfileResponse>> GetProfile(CancellationToken ct);


    [Put(ApiRoutes.Doctor.UpsertProfile)]
    Task<ApiResponse<object>> UpsertProfile([Body] UpsertDoctorProfileRequest body, CancellationToken ct);
}