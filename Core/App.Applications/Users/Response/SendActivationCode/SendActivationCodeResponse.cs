namespace App.Applications.Users.Response.SendActivationCode;

public class SendActivationCodeResponse
{
    public string SentDateTime { get; set; }

    public bool Success { get; set; }

    public string Message { get; set; }
}