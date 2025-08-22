namespace AppointmentPlanner.Client.Services.Auth;

public interface IAuthService
{
    Task<bool> LoginAsync(string             email , string password , CancellationToken ct = default);
    Task       LogoutAsync(CancellationToken ct                                                               = default);
    Task<bool> RegisterAsync(string          email , string password , string fullName , CancellationToken ct = default);
}