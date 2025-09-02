using App.Applications.MedicalRecords.Requests;
using MediatR;

namespace App.Applications.MedicalRecords.CommandQueries;

public record ListPrescriptionsByPatientQuery(
    long PatientUserId,
    long? UserId,
    string Role
) : IRequest<List<PrescriptionResponse>>;