using MediatR;

namespace App.Applications.Users.Requests.UserInfos;

public record UserInfoRequest() : IRequest<UserInfoResponse>;