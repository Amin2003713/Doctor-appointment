using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentPlanner.Shared.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentPlanner.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<User>   userManager ,
    SignInManager<User> signInManager ,
    IConfiguration      config
)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var exists = await userManager.FindByEmailAsync(dto.Email);
        if (exists is not null) return BadRequest("Email already registered");

        var user = new User
        {
            UserName = dto.Email , Email = dto.Email , FullName = dto.FullName
        };

        var result = await userManager.CreateAsync(user , dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok("Registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null) return Unauthorized();

        var passCheck = await signInManager.CheckPasswordSignInAsync(user , dto.Password , lockoutOnFailure: false);
        if (!passCheck.Succeeded) return Unauthorized();

        var token = await GenerateJwtAsync(user);
        return Ok(token);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // JWT logout is client-side (remove token). This endpoint is here if you later add refresh tokens / server revocation.
        return Ok();
    }

    private async Task<AuthTokenResponse> GenerateJwtAsync(User user)
    {
        var jwt   = config.GetSection("Jwt");
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub , user.Id.ToString()) , new Claim(JwtRegisteredClaimNames.Email , user.Email ?? "") ,
            new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) , new(ClaimTypes.Name , user.UserName           ?? "") ,
            new(ClaimTypes.NameIdentifier , user.Id.ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role , r)));

        var expires = DateTime.UtcNow.AddHours(8);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"] ,
            audience: jwt["Audience"] ,
            claims: claims ,
            expires: expires ,
            signingCredentials: creds);

        return new AuthTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token) , ExpiresAtUtc = expires , User = new MinimalUser
            {
                Id = user.Id , Email = user.Email! , FullName = user.FullName
            }
        };
    }
}

public record RegisterDto(string Email , string Password , string FullName);

public record LoginDto(string Email , string Password);

public class AuthTokenResponse
{
    public string AccessToken { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
    public MinimalUser User { get; set; } = new();
}

public class MinimalUser
{
    public long Id { get; set; }
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
}