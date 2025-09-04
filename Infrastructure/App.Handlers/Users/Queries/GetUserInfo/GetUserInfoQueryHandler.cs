using App.Applications.Users.Queries.GetUserInfo;
using App.Domain.Users;

namespace App.Handlers.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(
    ILocalStorage repository,
    ILogger<GetUserInfoQueryHandler> logger
)
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