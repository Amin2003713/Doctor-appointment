using App.Applications.Panel.Response.CheckDomain;
using MediatR;
using MudBlazor;

namespace App.Applications.Panel.Requests.CheckDomain;

public class CheckDomainRequest : IRequest<CheckDomainResponse>
{
    [Label("شناسه شرکت")] public string Domain { get; set; }

    public static CheckDomainRequest Create(string domain)
    {
        return new CheckDomainRequest
        {
            Domain = domain ,
        };
    }
}