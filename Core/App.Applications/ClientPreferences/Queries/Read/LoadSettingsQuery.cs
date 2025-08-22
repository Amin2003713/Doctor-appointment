using App.Common.General.BaseCommandQuery;
using App.Domain.ClientPreferences;
using MediatR;

namespace App.Applications.ClientPreferences.Queries.Read;

public class LoadSettingsQuery : BaseCommandQuery , IRequest<Settings>
{
}