using System.Net;
using App.Applications.Users.Apis;
using App.Applications.Users.Commands.Update;

using App.Applications.Users.Requests.Login;
using App.Applications.Users.Response.Login;
using App.Common.General.ApiResult;
using App.Common.Utilities.Converter;
using App.Domain.Users;
using App.Persistence.Services.Refit;
using Mapster;
using MediatR;

namespace App.Handlers.Users.Requests.RefreshToken;

public class RefreshTokenRequestHandler(
    ApiFactory apiFactory , IMediator mediator) : IRequestHandler<RefreshTokenRequest , LoginResponse>
{
    private readonly IUserApis _apis = apiFactory.CreateApi<IUserApis>();

    public async Task<LoginResponse> Handle(RefreshTokenRequest request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request , nameof(request));
        ArgumentNullException.ThrowIfNull(_apis ,   nameof(_apis));


        var result = (await _apis.RefreshToken(request)).DeserializeOrThrow<LoginResponse>();

        var user = result?.CreateUser();


        await mediator.Send(user.Adapt<UpdateUserInfoCommand>() , cancellationToken);
        return result!;
    }
}