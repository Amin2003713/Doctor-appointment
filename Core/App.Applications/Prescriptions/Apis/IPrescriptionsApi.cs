using App.Applications.Prescriptions.Requests;
using App.Applications.Prescriptions.Responses;
using Refit;

namespace App.Applications.Prescriptions.Apis;

public interface IPrescriptionsApi
{
    [Post("/api/prescriptions")]
    Task<PrescriptionResponse> Create([Body] CreatePrescriptionRequest body);

    [Get("/api/prescriptions/{id}")]
    Task<PrescriptionResponse> GetById(Guid id);

    [Get("/api/prescriptions/by-patient/{patientUserId}")]
    Task<List<PrescriptionResponse>> ListByPatient(long patientUserId);

    [Put("/api/prescriptions/{id}/cancel")]
    Task Cancel(Guid id);

    [Put("/api/prescriptions/{id}/dispense")]
    Task Dispense(Guid id);
}