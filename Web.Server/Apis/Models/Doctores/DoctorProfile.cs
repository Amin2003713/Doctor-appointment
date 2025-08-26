namespace Api.Endpoints.Models.Doctores;

public class DoctorProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = default!;
    public string Title { get; set; } = "MD";               // e.g., Cardiologist, Orthopedic Surgeon
    public string Biography { get; set; } = default!;
    public string[] Specialties { get; set; } = Array.Empty<string>();
    public string[] Education { get; set; } = Array.Empty<string>(); // e.g., ["MD – Tehran Univ.", "Fellowship – ..."]
    public string[] Languages { get; set; } = Array.Empty<string>(); // e.g., ["fa-IR","en-US"]
    public int YearsOfExperience { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? WhatsApp { get; set; }
}