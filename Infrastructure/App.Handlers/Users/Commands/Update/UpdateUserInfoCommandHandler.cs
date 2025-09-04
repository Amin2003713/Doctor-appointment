using App.Applications.Users.Commands.Update;
using App.Domain.Users;

namespace App.Handlers.Users.Commands.Update;

public class UpdateUserInfoCommandHandler(
    ILocalStorage repository  ,
    ClientStateProvider provider
)
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