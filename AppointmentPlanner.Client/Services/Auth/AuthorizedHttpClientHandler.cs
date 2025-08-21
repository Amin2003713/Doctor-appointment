using Blazored.LocalStorage;

namespace AppointmentPlanner.Client.Services.Auth;

public class AuthorizedHttpClientHandler (
    ILocalStorageService local
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await local.GetItemAsStringAsync("access_token", cancellationToken);
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}