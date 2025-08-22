namespace App.Applications.Users.Requests.Verify;

public class VerifyPhoneNumberRequest
{
    public string PhoneNumber { get; set; }

    public string Code { get; set; }
}