using App.Applications.Users.Apis;
using App.Applications.Users.Commands.Update;
using App.Applications.Users.Requests.GetSecretaries;
using App.Applications.Users.Requests.Login;
using App.Applications.Users.Requests.UserQueries;
using App.Applications.Users.Response.Login;
using App.Common.General.ApiResult;
using App.Common.Utilities.Converter;
using App.Persistence.Services.AuthState;
using App.Persistence.Services.Refit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Components;
using Refit;

namespace App.Handlers.Users.Requests.GetSecretaries;

public class GetSecretariesRequestHandler : IRequestHandler<GetSecretariesRequest, UserListItemResponse>