using App.Applications.Users.Apis;
using App.Applications.Users.Requests.ForgotPassword;
using App.Applications.Users.Requests.Login;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Users.Requests.ForgotPassword;

public class ForgotPasswordRequestHandler(
    ApiFactory factory,
    ISnackbarService service,
    IMediator mediator
)  : IRequestHandler<ResetPasswordRequest>
{
    private readonly IUserApis _apis = factory.CreateApi<IUserApis>();

    public async Task Handle(ResetPasswordRequest request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var resetPass = await _apis.ForgotPassword(request);
            if (resetPass.IsSuccessStatusCode)
                await mediator.Send(new LoginRequest()
                    {
                        Password = request.Password,
                        PhoneNumber = request.PhoneNumber
                    },
                    cancellationToken);
            else
                service.ShowError("Login failed");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}