
using App.Applications.Users.Queries.GetUserInfo;
using App.Common.Utilities.Storage;
using App.Domain.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Handlers.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(ILocalStorage repository , ILogger<GetUserInfoQueryHandler> logger) : IRequestHandler<GetUserInfoQuery , UserInfo?>
{
    public async Task<UserInfo?> Handle(GetUserInfoQuery request , CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAsync<UserInfo>(nameof(UserInfo));
            return (await Task.FromResult(result?.Id == Guid.Empty ? null : result))!;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message , e);
            throw;
        }
    }
}