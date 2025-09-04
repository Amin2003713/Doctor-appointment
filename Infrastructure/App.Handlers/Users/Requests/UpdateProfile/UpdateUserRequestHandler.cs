using App.Applications.Users.Requests.UpdateUser;

namespace App.Handlers.Users.Requests.UpdateProfile;

public class UpdateUserRequestHandler(
    ILogger<UpdateUserRequestHandler> logger,
    ApiFactory apiFactory,
    ISnackbarService snackbarService
) : IRequestHandler<UpdateUserRequest>
{
    private readonly IUserApis Apis = apiFactory.CreateApi<IUserApis>();

    public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiResult = await Apis.UpdateUser(request , request.userId );


            if (!apiResult.IsSuccessStatusCode)
            {
                logger.LogWarning("UpdateUser failed: StatusCode={Status}, Error={Error}",
                    apiResult.StatusCode,
                    apiResult.Error?.Message);

                return ;
            }

            snackbarService.ShowSuccess($"اطلاعات پروفایل {request.LastName} موفقیت به‌روزرسانی شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating profile.");
            throw;
        }
    }
}