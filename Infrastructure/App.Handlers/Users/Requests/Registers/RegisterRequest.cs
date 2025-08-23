using App.Applications.Users.Apis;
using App.Persistence.Services.AuthState;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace App.Applications.Users.Requests.Registers.Patient;

public record RegisterRequestHandler(
    ApiFactory apiFactory,
    ClientStateProvider stateProvider,
    NavigationManager navigationManager
) : IRequestHandler<RegisterRequest>
{
    private IUserApis Apis { get; } = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        
        var result = await Apis.Register(request);
        if (!result.IsSuccessful)
            throw new Exception("wrong");

        var u = request.PhoneNumber;

        navigationManager.NavigateTo($"/Login?registered=1&u={Uri.EscapeDataString(u ?? string.Empty)}", forceLoad: false);
    }

}