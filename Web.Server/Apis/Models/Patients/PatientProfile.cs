namespace Api.Endpoints.Models.Patients;

public class PatientProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } 
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? NationalId { get; set; }
    public string? InsuranceNumber { get; set; }
}