using App.Applications.Prescriptions.Requests;
using App.Applications.Prescriptions.Responses;
using Refit;

namespace App.Applications.Prescriptions.Apis;

public interface IPrescriptionsApi
{
    [Post("/api/prescriptions")]
    Task<ApiResponse<PrescriptionResponse>> Create([Body] CreatePrescriptionRequest body);

    [Get("/api/prescriptions/{id}")]
    Task<ApiResponse<PrescriptionResponse>> GetById(Guid id);

    [Get("/api/prescriptions/by-patient/{patientUserId}")]
    Task<ApiResponse<List<PrescriptionResponse>>> ListByPatient(long patientUserId);

    [Put("/api/prescriptions/{id}/cancel")]
    Task<ApiResponse<object>> Cancel(Guid id);

    [Put("/api/prescriptions/{id}/dispense")]
    Task<ApiResponse<object>> Dispense(Guid id);
}