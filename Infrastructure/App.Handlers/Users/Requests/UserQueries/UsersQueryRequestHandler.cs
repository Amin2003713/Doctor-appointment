using App.Applications.Users.Apis;
using App.Applications.Users.Requests.UserQueries;
using App.Common.General.ApiResult;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Users.Requests.UserQueries;

public class UsersQueryRequestHandler(
    ApiFactory apiFactory
) : IRequestHandler<UsersQueryRequest , PagedResult<UserListItemResponse>>
{
    public IUserApis Apis { get; set; } = apiFactory.CreateApi<IUserApis>();

    public async Task<PagedResult<UserListItemResponse>> Handle(UsersQueryRequest request, CancellationToken cancellationToken)
    {
        PagedResult<UserListItemResponse> result = null! ;

        if (request.IsSecretaries)
        {
            var apiResult = await Apis.GetSecretaries(request);
            if (!apiResult.IsSuccessStatusCode || apiResult.Content == null)
                return default!;

            result = apiResult.Content;
        }
        else
        {
            var apiResult = await Apis.GetUsers(request);
            if (!apiResult.IsSuccessStatusCode || apiResult.Content == null)
                return default!;

            result = apiResult.Content;
        }

        return result;
    }
}