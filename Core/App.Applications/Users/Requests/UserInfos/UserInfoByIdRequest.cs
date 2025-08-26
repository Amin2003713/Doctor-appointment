using MediatR;

namespace App.Applications.Users.Requests.UserInfos;

public record UserInfoByIdRequest(
    long Id
) : IRequest<UserInfoResponse>;