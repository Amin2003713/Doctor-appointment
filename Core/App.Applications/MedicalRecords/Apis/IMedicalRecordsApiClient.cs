using App.Applications.MedicalRecords.Requests;
using Refit;

namespace App.Applications.MedicalRecords.Apis;

public interface IMedicalRecordsApiClient
{
    // MedicalRecordsController Endpoints
    [Get("/api/medical-records/{patientUserId}/ehr")]
    Task<PatientEhrResponse> GetEhrAsync(long patientUserId, CancellationToken ct = default);

    [Post("/api/medical-records")]
    Task<MedicalRecordResponse> UpsertMedicalRecordAsync([Body] UpsertMedicalRecordRequest body, CancellationToken ct = default);

    [Get("/api/medical-records/by-appointment/{appointmentId}")]
    Task<List<MedicalRecordResponse>> GetMedicalRecordsByAppointmentAsync(Guid appointmentId, CancellationToken ct = default);

    // PrescriptionsController Endpoints
    [Post("/api/prescriptions")]
    Task<PrescriptionResponse> CreatePrescriptionAsync([Body] CreatePrescriptionRequest body, CancellationToken ct = default);

    [Get("/api/prescriptions/{id}")]
    Task<PrescriptionResponse> GetPrescriptionByIdAsync(Guid id, CancellationToken ct = default);

    [Get("/api/prescriptions/by-patient/{patientUserId}")]
    Task<List<PrescriptionResponse>> ListPrescriptionsByPatientAsync(long patientUserId, CancellationToken ct = default);

    [Put("/api/prescriptions/{id}/cancel")]
    Task CancelPrescriptionAsync(Guid id, CancellationToken ct = default);

    [Put("/api/prescriptions/{id}/dispense")]
    Task MarkPrescriptionDispensedAsync(Guid id, CancellationToken ct = default);
}