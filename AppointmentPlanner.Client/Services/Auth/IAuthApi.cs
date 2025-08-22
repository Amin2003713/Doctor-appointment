using AppointmentPlanner.Shared.AuthModels;
using Refit;

namespace AppointmentPlanner.Client.Services.Auth;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<AuthTokenResponse> LoginAsync([Body] LoginRequest request , CancellationToken ct = default);

    [Post("/api/auth/refresh")]
    Task<AuthTokenResponse> RefreshAsync([Body] object body , CancellationToken ct = default); // anonymous type still supported

    [Post("/api/auth/register")]
    Task<HttpResponseMessage> RegisterAsync([Body] RegisterRequest request , CancellationToken ct = default);
}