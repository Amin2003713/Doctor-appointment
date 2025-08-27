using System;
using System.Threading;
using System.Threading.Tasks;
using App.Applications.Clinic.Apis;
using App.Applications.Clinic.Requests.Get; // GetClinicSettingsInfo, ClinicSettingsResponse
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Clinic.Requests.Get
{
    public class GetClinicSettingsInfoHandler(
        ApiFactory factory,
        ISnackbarService snackbar
    ) : IRequestHandler<GetClinicSettingsInfo, ClinicSettingsResponse>
    {
        private readonly IClinicApis _apis = factory.CreateApi<IClinicApis>();

        public async Task<ClinicSettingsResponse> Handle(GetClinicSettingsInfo request, CancellationToken cancellationToken)
        {
            try
            {
                var resp = await _apis.GetSettings(cancellationToken);
                if (resp.IsSuccessStatusCode && resp.Content is not null)
                    return resp.Content;

                snackbar.ShowApiResult(resp.StatusCode);
                return null!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                snackbar.ShowError("خطا در دریافت تنظیمات کلینیک");
                throw;
            }
        }
    }
}