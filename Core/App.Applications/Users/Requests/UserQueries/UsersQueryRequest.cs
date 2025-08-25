using App.Common.General.ApiResult;
using MediatR;

namespace App.Applications.Users.Requests.UserQueries;

public record UsersQueryRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null ,
    bool IsSecretaries = false
) : IRequest<PagedResult<UserListItemResponse>>;