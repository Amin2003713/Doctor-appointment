using App.Applications.Users.Apis;
using App.Applications.Users.Queries.GetUserInfo;
using App.Common.Utilities.Storage;
using App.Domain.Users;
using App.Persistence.Services.Refit;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Handlers.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(ILocalStorage repository, ILogger<GetUserInfoQueryHandler> logger)
    : IRequestHandler<GetUserInfoQuery, UserInfo?>
{

    public async Task<UserInfo?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAsync<UserInfo>(nameof(UserInfo));

            if (result != null && result?.Id != "")
                return result;

            return null;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return null;
        }
    }
}