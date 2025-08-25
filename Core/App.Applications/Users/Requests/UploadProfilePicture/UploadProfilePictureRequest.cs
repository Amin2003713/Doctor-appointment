using MediatR;
using Microsoft.AspNetCore.Components.Forms;

public record UploadProfilePictureRequest(
    IBrowserFile? Profile
) : IRequest<string>;