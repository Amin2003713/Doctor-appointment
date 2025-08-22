namespace AppointmentPlanner.Shared.Dtos;

public class AuthTokenResponse
{
    public string AccessToken { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
    public MinimalUser User { get; set; } = new();
}