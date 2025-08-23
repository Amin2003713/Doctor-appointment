using MediatR;

public record UserInfoRequest() : IRequest<UserInfoResponse>;