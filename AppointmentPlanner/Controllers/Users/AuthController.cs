using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentPlanner.Shared.AuthModels;
using AppointmentPlanner.Shared.Dtos;
using AppointmentPlanner.Shared.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthTokenResponse = AppointmentPlanner.Shared.AuthModels.AuthTokenResponse;

namespace AppointmentPlanner.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<User>   userManager ,
    SignInManager<User> signInManager ,
    IConfiguration      config
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
            return BadRequest("این ایمیل قبلاً ثبت شده است.");

        var user = new User
        {
            UserName = dto.Email , Email = dto.Email , FullName = dto.FullName
        };

        var result = await userManager.CreateAsync(user , dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        return Ok("ثبت‌نام با موفقیت انجام شد.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("ایمیل یا رمز عبور اشتباه است.");

        var passwordValid = await signInManager.CheckPasswordSignInAsync(user , dto.Password , lockoutOnFailure: false);

        if (!passwordValid.Succeeded)
            return Unauthorized("ایمیل یا رمز عبور اشتباه است.");

        var tokenResponse = await GenerateJwtAsync(user);
        return Ok(tokenResponse);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // JWT logout is client-side (token removal). Keep endpoint for future use.
        return Ok("خروج با موفقیت انجام شد.");
    }

    private async Task<AuthTokenResponse> GenerateJwtAsync(User user)
    {
        var jwtSection = config.GetSection("Jwt");
        var key        = jwtSection["Key"];
        var issuer     = jwtSection["Issuer"];
        var audience   = jwtSection["Audience"];

        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Missing JWT configuration.");

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub , user.Id.ToString()) , new(JwtRegisteredClaimNames.Email , user.Email ?? "") ,
            new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) , new(ClaimTypes.Name , user.UserName     ?? "") ,
            new(ClaimTypes.NameIdentifier , user.Id.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role , role)));

        var expires    = DateTime.UtcNow.AddHours(8);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds      = new SigningCredentials(signingKey , SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer ,
            audience: audience ,
            claims: claims ,
            expires: expires ,
            signingCredentials: creds
        );

        return new AuthTokenResponse(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(token) ,
            RefreshToken: null , // optional: implement refresh later
            ExpiresAtUtc: expires ,
            User: new AuthUser(
                Id: user.Id ,
                Email: user.Email       ?? "" ,
                FullName: user.FullName ?? ""
            )
        );
    }
}