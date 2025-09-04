using App.Applications.ClientPreferences.Commands.Update;
using App.Applications.ClientPreferences.Queries.Read;
using App.Domain.ClientPreferences;

namespace App.Handlers.ClientPreferences.Queries.Read;

public class LoadSettingsQueryHandler(
    ILocalStorage repository ,
    IMediator mediator
) : IRequestHandler<LoadSettingsQuery , Settings>
{
    public async Task<Settings> Handle(LoadSettingsQuery request , CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAsync<Settings>(nameof(Settings));

            if (result == null)
                await mediator.Send(UpdateSettingCommand.CreateDefault() , cancellationToken);

            return result ?? UpdateSettingCommand.CreateDefault().Adapt<Settings>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}