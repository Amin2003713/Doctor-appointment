// Application/Schedule/Dto.cs

namespace Api.Endpoints.Dtos.Schedules;

public class WorkScheduleResponse
{
    public List<WorkingDayDto> Days { get; set; } = new();
    public List<SpecialDateOverrideDto> Overrides { get; set; } = new();
}