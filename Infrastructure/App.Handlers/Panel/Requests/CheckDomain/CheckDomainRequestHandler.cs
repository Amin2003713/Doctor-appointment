using App.Applications.Panel.Apis;
using App.Applications.Panel.Requests.CheckDomain;
using App.Applications.Panel.Response.CheckDomain;
using App.Common.Utilities.Converter;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Panel.Requests.CheckDomain;

public class CheckDomainRequestHandler(ApiFactory factory ) : IRequestHandler<CheckDomainRequest , CheckDomainResponse>
{
    private IPanelApi api { get; } = factory.CreateApi<IPanelApi>();

    public async Task<CheckDomainResponse> Handle(CheckDomainRequest request , CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request , nameof(request));

        try
        {
            return (await api.Check_Domain(request.Domain)).DeserializeOrThrow<CheckDomainResponse>();
            

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}