  using App.Applications.Appointments.Requests;

  // Request/Response Models for MedicalRecordsController
  namespace App.Applications.MedicalRecords.Requests;

  public class AppointmentSummaryResponse
  {
      public Guid Id { get; set; }
      public DateTime Date { get; set; }
      public string Start { get; set; } = null!;
      public string End { get; set; } = null!;
      public AppointmentStatus Status { get; set; }
      public string? Notes { get; set; }
  }

  // Request Models for PrescriptionsController