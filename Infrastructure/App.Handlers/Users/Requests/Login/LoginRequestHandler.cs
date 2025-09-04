using App.Applications.Users.Commands.Update;
using App.Applications.Users.Requests.Login;
using App.Applications.Users.Response.Login;
using Microsoft.AspNetCore.Components;

namespace App.Handlers.Users.Requests.Login;

public class LoginRequestHandler(
    ApiFactory apiFactory ,
    ClientStateProvider stateProvider ,
    NavigationManager navigationManager ,
    IMediator mediator
) : IRequestHandler<LoginRequest , LoginResponse>
{
    public IUserApis Apis { get; set; } = apiFactory.CreateApi<IUserApis>();

    public async Task<LoginResponse> Handle(LoginRequest request , CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis,    nameof(Apis));

            request.PhoneNumber = request.PhoneNumber.Trim();
            var result = await Apis.Login(request);

            if (!result.IsSuccessStatusCode)
            {
                result.DeserializeAndThrow();
                return null!;
            }


            var user = result?.Content!.CreateUser();
            await mediator.Send(user.Adapt<UpdateUserInfoCommand>(), cancellationToken);

            await stateProvider.GetAuthenticationStateAsync();
            navigationManager.NavigateTo("/", true);
            return result?.Content!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}