using App.Applications.Users.Apis;
using App.Applications.Users.Requests.UploadProfilePicture;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.Refit;
using MediatR;
using Refit;

namespace App.Handlers.Users.Requests.UploadProfilePicture;

public class UploadProfilePictureRequestHandler(
    ApiFactory          apiFactory,
    ISnackbarService    SnackbarService
)  : IRequestHandler< UploadProfilePictureRequest , string>
{
    private IUserApis Apis { get; } = apiFactory.CreateApi<IUserApis>();

    public async Task<string> Handle(UploadProfilePictureRequest request, CancellationToken cancellationToken)
    {
        if (request.Profile is null)
            SnackbarService.ShowError("Profile is null");

        await using var readStream = request.Profile?.OpenReadStream(request.Profile.Size);
        var             ms         = new MemoryStream();
        await readStream!.CopyToAsync(ms, cancellationToken);
        ms.Position = 0;

        var imageUrl = await Apis.UploadAvatar(new StreamPart(ms, request.Profile.Name, request.Profile.ContentType));

        if (imageUrl.IsSuccessStatusCode)
            return imageUrl.Content!;

        SnackbarService.ShowError("Profile upload failed");
        return null!;
    }
}