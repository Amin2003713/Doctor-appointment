using App.Applications.Appointments.Requests;
using Refit;

namespace App.Applications.Appointments.Apis;

public interface IAppointmentsApi
{
// POST /api/appointments -> Guid
    [Post("/api/appointments")]
    Task<Guid> CreateAsync([Body] UpsertAppointmentRequest body, CancellationToken ct = default);


// GET /api/appointments
    [Get("/api/appointments")]
    Task<List<AppointmentResponse>> ListAsync(
        [AliasAs("from")]          DateOnly? from          = null,
        [AliasAs("to")]            DateOnly? to            = null,
        [AliasAs("patientUserId")] long?     patientUserId = null,
        CancellationToken                    ct            = default);


// GET /api/appointments/{id}
    [Get("/api/appointments/{id}")]
    Task<AppointmentResponse> GetByIdAsync(Guid id, CancellationToken ct = default);


// PUT /api/appointments/{id}/cancel
    [Put("/api/appointments/{id}/cancel")]
    Task<ApiResponse<object>> CancelAsync(Guid id, CancellationToken ct = default);


// PUT /api/appointments/{id}/complete
    [Put("/api/appointments/{id}/complete")]
    Task<ApiResponse<object>> CompleteAsync(Guid id, CancellationToken ct = default);


// PUT /api/appointments/{id}/reschedule
    [Put("/api/appointments/{id}/reschedule")]
    Task<ApiResponse<object>> RescheduleAsync(Guid id, [Body] UpsertAppointmentRequest body, CancellationToken ct = default);
}