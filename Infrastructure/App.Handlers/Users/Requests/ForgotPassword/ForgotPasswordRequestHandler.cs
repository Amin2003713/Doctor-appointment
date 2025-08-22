using App.Applications.Users.Apis;
using App.Applications.Users.Requests.ForgotPassword;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Users.Requests.ForgotPassword;

public class ForgotPasswordRequestHandler(ApiFactory factory )  : IRequestHandler<ForgotPasswordRequest>
{
    private readonly IUserApis _apis = factory.CreateApi<IUserApis>();

    public async Task Handle(ForgotPasswordRequest request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
             await _apis.ForgotPassword(request , request.UserName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}