using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AppointmentPlanner.Client.Services.Auth;

public static class JwtUtils
{
    private readonly static JwtSecurityTokenHandler Handler = new();

    public static ClaimsIdentity BuildIdentity(string jwt , string authenticationType = "jwt")
    {
        if (string.IsNullOrWhiteSpace(jwt)) return new ClaimsIdentity();

        var token  = Handler.ReadJwtToken(jwt);
        var claims = token.Claims.ToList();

        // Optionally map standard claims → ClaimTypes.Name etc.
        if (claims.Any(c => c.Type == ClaimTypes.Name))
            return new ClaimsIdentity(claims , authenticationType);

        var email = claims.FirstOrDefault(c => c.Type is "email" or ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email))
            claims.Add(new Claim(ClaimTypes.Name , email));

        return new ClaimsIdentity(claims , authenticationType);
    }

    public static bool IsExpired(DateTime expiresAtUtc , TimeSpan? skew = null)
    {
        var skewVal = skew ?? TimeSpan.FromMinutes(1); // default skew
        return DateTime.UtcNow.Add(skewVal) >= expiresAtUtc;
    }
}