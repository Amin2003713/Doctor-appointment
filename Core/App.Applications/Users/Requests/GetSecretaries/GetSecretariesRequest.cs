using App.Applications.Users.Requests.UserQueries;
using App.Applications.Users.Response.Login;
using MediatR;
using MudBlazor;

namespace App.Applications.Users.Requests.GetSecretaries;

public record GetSecretariesRequest() : IRequest<UserListItemResponse>;