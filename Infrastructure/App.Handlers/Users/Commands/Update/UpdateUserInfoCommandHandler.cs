using App.Applications.Users.Commands.Update;
using App.Common.Utilities.Storage;
using App.Domain.Users;
using App.Persistence.Services.AuthState;
using Mapster;
using MediatR;

namespace App.Handlers.Users.Commands.Update;

public class UpdateUserInfoCommandHandler(ILocalStorage repository  , ClientStateProvider provider)
    : IRequestHandler<UpdateUserInfoCommand>
{
    public async Task Handle(UpdateUserInfoCommand request , CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request , nameof(request));

            var user = request.Adapt<UserInfo>();

            await repository.UpdateAsync(nameof(UserInfo) , user);

            provider.User = user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}