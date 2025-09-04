using App.Applications.Users.Requests.UpdateUser;

namespace App.Handlers.Users.Requests.UpdateProfile;

public class UpdateProfileRequestHandler(
    ILogger<UpdateProfileRequestHandler> logger,
    ApiFactory apiFactory,
    ISnackbarService snackbarService
) : IRequestHandler<UpdateProfileRequest>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiResult = await Apis.UpdateProfile(request);


            if (!apiResult.IsSuccessStatusCode)
            {
                logger.LogWarning("UpdateProfile failed: StatusCode={Status}, Error={Error}",
                    apiResult.StatusCode,
                    apiResult.Error?.Message);

                return ;
            }


            snackbarService.ShowSuccess("اطلاعات پروفایل با موفقیت به‌روزرسانی شد.");

            var me = await Apis.Me();


            if (!me.IsSuccessStatusCode)
                return;

            Console.WriteLine("user updated");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating profile.");
            throw;
        }
    }
}