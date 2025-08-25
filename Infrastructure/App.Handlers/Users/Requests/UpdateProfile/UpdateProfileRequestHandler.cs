using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using App.Applications.Users.Apis;
using App.Applications.Users.Queries.GetUserInfo;
using App.Applications.Users.Requests.UpdateUser;
using App.Common.Utilities.Snackbar;
using App.Common.Utilities.Storage;
using App.Domain.Users;
using App.Persistence.Services.Refit;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Handlers.Users.Requests.UpdateProfile
{
    public class UpdateProfileRequestHandler(
        ILocalStorage repository,
        ILogger<UpdateProfileRequestHandler> logger,
        ApiFactory apiFactory,
        ISnackbarService snackbarService
    ) : IRequestHandler<UpdateProfileRequest>
    {
        // ساخت Refit API
        private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

        public async Task Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
             
                var apiResult = await Apis.UpdateProfile(request);

                if (!apiResult.IsSuccessful)
                {
                    logger.LogWarning("UpdateProfile failed: StatusCode={Status}, Error={Error}",
                        apiResult.StatusCode, apiResult.Error?.Message);

                    return ;
                }

                snackbarService.ShowSuccess(("اطلاعات پروفایل با موفقیت به‌روزرسانی شد."));

                // پس از موفقیت، اطلاعات کاربر را تازه بگیر و در LocalStorage ذخیره کن
                var me = await Apis.Me();

                if (!me.IsSuccessful || me.Content is null)
                    return;

                var info = me.Content.Adapt<UserInfo>();
                await repository.UpdateAsync(nameof(UserInfo), info);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while updating profile.");
                throw;
            }
        }
    }
}
