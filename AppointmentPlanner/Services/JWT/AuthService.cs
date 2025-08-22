using AppointmentPlanner.Client.Services.Auth;

namespace AppointmentPlanner.Services.JWT;

public sealed class PrerenderAuthStub : IAuthService
{
    public Task<bool> IsAuthenticatedAsync()
        => Task.FromResult(false);

    public Task LogoutAsync()
        => Task.CompletedTask;
    // add any other members your interface requires with harmless defaults
    public Task<bool> LoginAsync(string email , string password , CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Task LogoutAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Task<bool> RegisterAsync(string email , string password , string fullName , CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}