namespace App.Applications.Doctor.Requests.Get;

public class DoctorProfileResponse
{
    public string FullName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Biography { get; set; } = default!;
    public string[] Specialties { get; set; } = Array.Empty<string>();
    public string[] Education { get; set; } = Array.Empty<string>();
    public string[] Languages { get; set; } = Array.Empty<string>();
    public int YearsOfExperience { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? WhatsApp { get; set; }
}