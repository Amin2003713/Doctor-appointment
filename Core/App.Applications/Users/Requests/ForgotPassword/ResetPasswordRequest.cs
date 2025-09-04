using MediatR;

namespace App.Applications.Users.Requests.ForgotPassword;

public class ResetPasswordRequest : IRequest
{
    public string PhoneNumber { get; set; }


    public string Password { get; set; }


    public string ConfirmPassword { get; set; }
}