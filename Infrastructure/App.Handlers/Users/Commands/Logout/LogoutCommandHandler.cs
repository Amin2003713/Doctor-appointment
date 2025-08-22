using App.Applications.Users.Commands.Logout;
using App.Applications.Users.Commands.Update;
using App.Applications.Users.Queries.GetUserInfo;
using Mapster;
using MediatR;

namespace App.Handlers.Users.Commands.Logout;

public class LogoutCommandHandler(IMediator mediator) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request , nameof(request));

        try
        {
            if (request.Token is null)
                return;

            var user = await mediator.Send(new GetUserInfoQuery() , cancellationToken);
            if (user == null)
                return;

            user.Token        = null!;
            user.RefreshToken = null!;

            await mediator.Send(user.Adapt<UpdateUserInfoCommand>() , cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}