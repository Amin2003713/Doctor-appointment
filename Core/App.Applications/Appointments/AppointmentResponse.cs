using Heron.MudCalendar;

namespace App.Applications.Appointments;

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceTitle { get; set; } = default!;
    public DateOnly Date { get; set; }
    public string Start { get; set; } = default!;
    public string End { get; set; } = default!;
    public AppointmentStatus Status { get; set; }
    public string PatientFullName { get; set; } = default!;
    public string PatientPhone { get; set; } = default!;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "ريال";
    public string? Notes { get; set; }
}

public class ClinicCalendarItem : CalendarItem
{
    public Guid AppointmentId { get; set; }

    // Extra fields for display
    public string ServiceTitle { get; set; } = "";
    public string PatientFullName { get; set; } = "";
    public string PatientPhone { get; set; } = "";
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = "";
    public string? Notes { get; set; }
    public AppointmentStatus Status { get; set; }

    public static ClinicCalendarItem FromAppointment(AppointmentResponse a)
    {
        return new ClinicCalendarItem
        {
            // Base CalendarItem requirements
            Start = DateTime.Parse(a.Start),
            End   = DateTime.Parse(a.End),
            Text  = a.ServiceTitle, // optional, used by MudCalendar

            // Keep original Id explicitly
            AppointmentId = a.Id,

            // Custom fields
            ServiceTitle    = a.ServiceTitle,
            PatientFullName = a.PatientFullName,
            PatientPhone    = a.PatientPhone,
            PriceAmount     = a.PriceAmount,
            PriceCurrency   = a.PriceCurrency,
            Notes           = a.Notes,
            Status          = a.Status
        };
    }

    public static List<ClinicCalendarItem> FromAppointments(IEnumerable<AppointmentResponse> appointments)
        => appointments.Select(FromAppointment).ToList();
}