using App.Applications.Users.Requests.UserInfos;
using App.Domain.Users;
using App.Handlers.Users.Requests.UpdateProfile;

namespace App.Handlers.Users.Requests.UserInfos;

public record UserInfoRequestHandler(
    ILocalStorage repository,
    ILogger<UpdateProfileRequestHandler> logger,
    ApiFactory apiFactory,
    ISnackbarService snackbarService,
    ClientStateProvider StateProvider
) : IRequestHandler< UserInfoRequest, UserInfoResponse>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task<UserInfoResponse> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await repository.GetAsync<UserInfo>(nameof(UserInfo));

            if (user?.Token is null )
            {
                await StateProvider.Logout();
                return null!;
            }

            var apiResult = await Apis.Me();

            if (!apiResult.IsSuccessStatusCode)
            {
                snackbarService.ShowApiResult(apiResult.StatusCode);
                return null!;
            }

            var newUser = apiResult.Content.Adapt<UserInfo>();
            newUser.Token = user.Token;
            await repository.UpdateAsync(nameof(UserInfo), user);
            await StateProvider.SetUserAsync(user);
            return apiResult.Content!;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}