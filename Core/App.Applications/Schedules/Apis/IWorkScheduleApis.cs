using App.Applications.Schedules.Requests.Get;
using App.Applications.Schedules.Requests.Update;
using App.Common.General;
using Refit;

namespace App.Applications.Schedules.Apis;

public interface IWorkScheduleApis
{
    
    [Get(ApiRoutes.Schedule.Get)]
    Task<ApiResponse<WorkScheduleResponse>> GetSchedule(CancellationToken ct = default);

    
    [Put(ApiRoutes.Schedule.Update)]
    Task<ApiResponse<object>> UpdateSchedule([Body] UpdateWorkScheduleRequest body, CancellationToken ct = default);

    
    [Get(ApiRoutes.Schedule.Slots)]
    Task<ApiResponse<List<string>>> GetSlots([Query] DateOnly date, [Query] Guid serviceId, CancellationToken ct = default);
}