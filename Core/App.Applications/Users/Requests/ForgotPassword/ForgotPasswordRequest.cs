using MediatR;

namespace App.Applications.Users.Requests.ForgotPassword;

public class ForgotPasswordRequest : IRequest
{
    /// <summary>
    /// The phone number associated with the user account.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// The new password to be set for the user account.
    /// </summary>
     public string NewPassword { get; set; }

    /// <summary>
    /// A confirmation of the new password (must match <see cref="NewPassword"/>).
    /// </summary>
    public string ConfirmPassword { get; set; }
}
