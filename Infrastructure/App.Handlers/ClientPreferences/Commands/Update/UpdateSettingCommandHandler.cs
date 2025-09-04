using App.Applications.ClientPreferences.Commands.Update;
using App.Domain.ClientPreferences;

namespace App.Handlers.ClientPreferences.Commands.Update;

public class UpdateSettingCommandHandler(
    ILocalStorage repository
) : IRequestHandler<UpdateSettingCommand>
{
    public Task Handle(UpdateSettingCommand request , CancellationToken cancellationToken)
    {
        repository.SetAsync(nameof(Settings) , request.Adapt<Settings>());
        return Task.CompletedTask;
    }
}