using MediatR;

namespace App.Applications.Users.Requests.ToggleUsers;

public record ToggleUserRequest(
    long UserId
) : IRequest;