using App.Applications.Users.Apis;
using App.Applications.Users.Requests.UserInfos;
using App.Common.Utilities.Snackbar;
using App.Common.Utilities.Storage;
using App.Persistence.Services.AuthState;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Handlers.Users.Requests.UserInfos;

public record UserInfoByIdRequestHandler(
    ILocalStorage repository,
    ILogger<UserInfoByIdRequestHandler> logger,
    ApiFactory apiFactory,
    ISnackbarService snackbarService,
    ClientStateProvider StateProvider
) : IRequestHandler< UserInfoByIdRequest, UserInfoResponse>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task<UserInfoResponse> Handle(UserInfoByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiResult = await Apis.GetUser(request.Id);

            if (apiResult.IsSuccessStatusCode)
                return apiResult.Content!;

            snackbarService.ShowApiResult(apiResult.StatusCode);
            return null!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}