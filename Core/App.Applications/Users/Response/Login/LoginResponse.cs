#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Domain.Users;

namespace App.Applications.Users.Response.Login;

#endregion

public class LoginResponse
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }


    public UserInfo CreateUser()
    {
        var handler  = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(Token);

        return new UserInfo
        {
            UserName      = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? string.Empty ,
            Id            = new Guid(jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value!) ,
            FirstName     = jwtToken.Claims.FirstOrDefault(c => c.Type == "firstName")?.Value ?? string.Empty ,
            LastName      = jwtToken.Claims.FirstOrDefault(c => c.Type == "lastName")?.Value  ?? string.Empty ,
            Profile       = jwtToken.Claims.FirstOrDefault(c => c.Type == "profile")?.Value ,
            LastLoginDate = jwtToken.Claims.FirstOrDefault(c => c.Type == "lastLoginDate")?.Value ?? string.Empty ,
            PhoneNumber   = jwtToken.Claims.FirstOrDefault(c => c.Type == "PhoneNumber")?.Value ,
            RolesList     = jwtToken.Claims.Where(c => c.Type          == ClaimTypes.Role).Select(c => c.Value).ToList() ,
            Token         = Token ,
            RefreshToken  = RefreshToken ,
        };
    }
}