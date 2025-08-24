using App.Domain.Users;
using MediatR;

namespace App.Applications.Users.Queries.GetUserInfo;

public class GetUserInfoQuery :  IRequest<UserInfo>
;