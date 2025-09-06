using Api.Endpoints.Dtos.Services;
using Api.Endpoints.Models.Appointments;

namespace Api.Endpoints.Dtos.Prescriptions;

public sealed class AppointmentSummary
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Start { get; set; } = default!;
    public string End { get; set; } = default!;
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public ServiceResponse  Service { get; set; }
}