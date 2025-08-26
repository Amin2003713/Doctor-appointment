namespace Api.Endpoints.Dtos.Users;

public class LoginResponseDto
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }
}