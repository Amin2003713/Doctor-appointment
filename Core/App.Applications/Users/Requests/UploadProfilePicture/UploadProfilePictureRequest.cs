using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace App.Applications.Users.Requests.UploadProfilePicture;

public record UploadProfilePictureRequest(
    IBrowserFile? Profile
) : IRequest<string>;