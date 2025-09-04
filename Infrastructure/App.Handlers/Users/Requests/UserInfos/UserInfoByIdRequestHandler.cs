using App.Applications.Users.Requests.UserInfos;

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