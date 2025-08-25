using App.Applications.Users.Apis;
using App.Applications.Users.Requests.UpdateUser;
using App.Applications.Users.Requests.UserInfos;
using App.Common.Utilities.Snackbar;
using App.Common.Utilities.Storage;
using App.Handlers.Users.Requests.UpdateProfile;
using App.Persistence.Services.Refit;
using MediatR;
using Microsoft.Extensions.Logging;

public record UserInfoRequestHandler(
    ILocalStorage repository,
    ILogger<UpdateProfileRequestHandler> logger,
    ApiFactory apiFactory,
    ISnackbarService snackbarService
) : IRequestHandler< UserInfoRequest, UserInfoResponse>
{
    public Task<UserInfoResponse> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

