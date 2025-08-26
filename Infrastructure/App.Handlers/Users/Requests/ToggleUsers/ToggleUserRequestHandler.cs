using App.Applications.Users.Apis;
using App.Applications.Users.Requests.ToggleUsers;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Users.Requests.ToggleUsers;

public class ToggleUserRequestHandler(
    ApiFactory apiFactory,
    ISnackbarService snackbarService
) : IRequestHandler<ToggleUserRequest>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(ToggleUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiResponse = await Apis.Toggle(request);

            if (apiResponse.IsSuccessStatusCode)
                snackbarService.ShowApiResult(apiResponse.StatusCode);
            else
                snackbarService.ShowError();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            snackbarService.ShowError("Something went wrong");
        }
    }
}