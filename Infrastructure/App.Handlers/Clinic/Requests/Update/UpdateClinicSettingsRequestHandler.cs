using System;
using System.Threading;
using System.Threading.Tasks;
using App.Applications.Clinic;
using App.Applications.Clinic.Apis;
using App.Applications.Clinic.Requests.Update;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;

namespace App.Handlers.Clinic.Requests.Update
{
    public class UpdateClinicSettingsRequestHandler(
        ApiFactory factory,
        ISnackbarService snackbar
    ) : IRequestHandler<UpdateClinicSettingsRequest>
    {
        private readonly IClinicApis _apis = factory.CreateApi<IClinicApis>();

        public async Task Handle(UpdateClinicSettingsRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var resp = await _apis.UpdateSettings(request, cancellationToken);

                if (resp.IsSuccessStatusCode)
                {
                    snackbar.ShowSuccess("تنظیمات با موفقیت ذخیره شد.");
                    return;
                }

                snackbar.ShowApiResult(resp.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                snackbar.ShowError("خطا در ذخیره تنظیمات کلینیک");
            }
        }
    }
}