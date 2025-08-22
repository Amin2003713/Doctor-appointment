#region

using App.Applications.Panel.Response.CheckDomain;
using Refit;

#endregion

namespace App.Applications.Panel.Apis;

public interface IPanelApi
{
    /// <summary>gets the company A.K.A Tenent info.</summary>
    /// <param name="domain">is the Identifier</param>
    /// <returns>A <see cref="Task" /> that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown when the request returns a non-success status code.</exception>
    [Get("/api/panel/check-domain")]
    Task<ApiResponse<CheckDomainResponse>> Check_Domain([Query] string domain);
}