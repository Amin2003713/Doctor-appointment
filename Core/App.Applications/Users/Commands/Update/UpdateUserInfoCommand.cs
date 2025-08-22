using App.Common.General.BaseCommandQuery;
using App.Domain.Users;
using MediatR;

namespace App.Applications.Users.Commands.Update;

public class UpdateUserInfoCommand : UserInfo , IRequest , IBaseCommandQuery
{
    public string Domain { get; set; }
}