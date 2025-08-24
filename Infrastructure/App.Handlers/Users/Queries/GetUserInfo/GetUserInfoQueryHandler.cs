using App.Applications.Users.Apis;
using App.Applications.Users.Queries.GetUserInfo;
using App.Common.Utilities.Storage;
using App.Domain.Users;
using App.Persistence.Services.Refit;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Handlers.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(ILocalStorage repository, ILogger<GetUserInfoQueryHandler> logger, ApiFactory apiFactory)
    : IRequestHandler<GetUserInfoQuery, UserInfo?>
{
    public readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task<UserInfo?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAsync<UserInfo>(nameof(UserInfo));

            if (result != null && result?.Id == "")
                return result;

            var apiResult = await Apis.Me();

            if (apiResult.IsSuccessful)
            {
                var info = apiResult.Content.Adapt<UserInfo>();
                await repository.UpdateAsync(nameof(UserInfo), info);
                return info;
            }

            return null;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            throw;
        }
    }
}