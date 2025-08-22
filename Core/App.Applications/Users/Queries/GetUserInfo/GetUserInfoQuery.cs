using App.Common.General.BaseCommandQuery;
using App.Domain.Users;
using MediatR;

namespace App.Applications.Users.Queries.GetUserInfo;

public class GetUserInfoQuery : BaseCommandQuery , IRequest<UserInfo>
;