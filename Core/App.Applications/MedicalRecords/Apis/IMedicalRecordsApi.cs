using App.Applications.DrugStore.Responses;
using App.Applications.MedicalRecords.Requests;
using App.Applications.MedicalRecords.Responses;
using Refit;

namespace App.Applications.MedicalRecords.Apis;

public interface IMedicalRecordsApi
{
    [Get("/api/medical-records/{patientUserId}/ehr")]
    Task<ApiResponse<PatientEhrResponse>> GetEhr(long patientUserId);

    [Post("/api/medical-records")]
    Task<ApiResponse<MedicalRecordResponse>> Upsert([Body] UpsertMedicalRecordRequest body);

    [Get("/api/medical-records/by-appointment/{appointmentId}")]
    Task<ApiResponse<List<MedicalRecordResponse>>> ByAppointment(Guid appointmentId);
}
