using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/User")]
public class UsersController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    RoleManager<IdentityRole<long>> roleManager,
    IOptions<JwtOptions> jwtOptions // bind from configuration
) : ControllerBase
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        // Ensure phone (username) is unique
        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null) return BadRequest("Phone number already registered.");

        var user = new AppUser
        {
            UserName = username,           // username = phone
            NormalizedUserName = username, // Identity will set too, but we align
            PhoneNumber = e164,
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
            FullName = dto.FullName
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        await EnsureRolesExist();
        await userManager.AddToRoleAsync(user, "Patient"); // self-registers as Patient

        return Ok();
    }

    [HttpPost("register/patient")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterOtherDto dto)
    {
        var (ok, msg) = await RegisterWithRole(dto, "Patient");
        return ok ? Ok() : BadRequest(msg);
    }

    [HttpPost("register/secretary")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> RegisterSecretary([FromBody] RegisterOtherDto dto)
    {
        var (ok, msg) = await RegisterWithRole(dto, "Secretary");
        return ok ? Ok() : BadRequest(msg);
    }

    [HttpPost("change-role")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto dto)
    {
        if (!new[]
            {
                "Patient",
                "Secretary"
            }.Contains(dto.NewRole))
            return BadRequest("Invalid role");

        if (!long.TryParse(dto.UserId, out var userId))
            return BadRequest("Invalid user id");

        var user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null) return NotFound("User not found");

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRoleAsync(user, dto.NewRole);

        return Ok();
    }

    [HttpGet("users")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<PagedResult<UserListItemDto>>> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        if (page <= 0) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;

        var q = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            q = q.Where(u =>
                (u.FullName != null && u.FullName.Contains(search)) ||
                (u.Email != null && u.Email.Contains(search)) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(search)) ||
                (u.UserName != null && u.UserName.Contains(search)));
        }

        var total = await Task.FromResult(q.LongCount()); // If using EF Core, use await q.LongCountAsync()

        var users = q
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList(); // If EF Core: await q.ToListAsync()

        // Load roles (simple N+1; fine for moderate page sizes)
        var list = new List<UserListItemDto>(users.Count);

        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            list.Add(new UserListItemDto(
                u.Id,
                u.PhoneNumber ?? u.UserName ?? "",
                u.Email,
                u.FullName,
                u.IsActive,
                u.CreatedAtUtc,
                u.LastLoginAtUtc,
                roles));
        }

        return Ok(new PagedResult<UserListItemDto>(list, page, pageSize, total));
    }

    [HttpGet("users/secretaries")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<PagedResult<UserListItemDto>>> GetSecretaries(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        if (page <= 0) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;

        // Pull all secretaries via Identity and then filter/page (works across providers)
        var secretaries = await userManager.GetUsersInRoleAsync("Secretary");

        var filtered = secretaries.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            filtered = filtered.Where(u =>
                (u.FullName != null && u.FullName.Contains(search)) ||
                (u.Email != null && u.Email.Contains(search)) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(search)) ||
                (u.UserName != null && u.UserName.Contains(search)));
        }

        var total = filtered.LongCount();

        var pageItems = filtered
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var list = new List<UserListItemDto>(pageItems.Count);

        foreach (var u in pageItems)
        {
            // Roles will include "Secretary" (and any others)
            var roles = await userManager.GetRolesAsync(u);
            list.Add(new UserListItemDto(
                u.Id,
                u.PhoneNumber ?? u.UserName ?? "",
                u.Email,
                u.FullName,
                u.IsActive,
                u.CreatedAtUtc,
                u.LastLoginAtUtc,
                roles));
        }

        return Ok(new PagedResult<UserListItemDto>(list, page, pageSize, total));
    }

// Already added before, shown here for completeness
    [HttpGet("user/{id:long}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<UserDetailDto>> GetUser(long id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound("User not found");

        var roles = await userManager.GetRolesAsync(user);

        var dto = new UserDetailDto(
            user.Id,
            user.PhoneNumber ?? user.UserName ?? "",
            user.Email,
            user.FullName ?? "",
            user.IsActive,
            user.CreatedAtUtc,
            user.LastLoginAtUtc,
            roles
        );

        return Ok(dto);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var user = await userManager.FindByNameAsync(username);
        if (user is not { IsActive: true }) return Unauthorized();

        var pwd = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
        if (!pwd.Succeeded) return Unauthorized();

        user.LastLoginAtUtc = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        var token = await GenerateJwtToken(user);
        return Ok(new
        {
            token
        });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordDto dto)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var user = await userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("User not found");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        // In a real system, you’d send an SMS; here we return the token for dev/testing.
        return Ok(new
        {
            token
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserInfoDto>> Me()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await userManager.GetRolesAsync(user);
        var dto = new UserInfoDto(
            user.Id,
            user.PhoneNumber ?? user.UserName ?? "",
            user.Email,
            user.FullName ?? "",
            roles.FirstOrDefault()
        );

        return Ok(dto);
    }

    // -------------------
    // Helpers
    // -------------------

    private async Task<(bool ok, string? error)> RegisterWithRole(RegisterOtherDto dto, string role)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null) return (false, "Phone number already registered.");

        var user = new AppUser
        {
            UserName = username,
            NormalizedUserName = username,
            PhoneNumber = e164,
            Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
            FullName = dto.FullName
        };

        var create = await userManager.CreateAsync(user, dto.Password);
        if (!create.Succeeded) return (false, string.Join("; ", create.Errors.Select(e => e.Description)));

        await EnsureRolesExist();
        await userManager.AddToRoleAsync(user, role);
        return (true, null);
    }

    [HttpPost("toggle")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> ToggleUser([FromBody] ToggleUserDto dto)
    {
        if (!long.TryParse(dto.UserId, out var userId))
            return BadRequest("Invalid user id");

        var user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null) return NotFound("User not found");

        user.IsActive = dto.IsActive;
        await userManager.UpdateAsync(user);

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.PhoneNumber,
            user.IsActive
        });
    }

    private async Task EnsureRolesExist()
    {
        foreach (var r in new[]
                 {
                     "Doctor",
                     "Secretary",
                     "Patient"
                 })
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole<long>(r));
    }

    private async Task<string> GenerateJwtToken(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName ?? user.UserName ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:   _jwt.Issuer,
            audience: _jwt.Audience,
            claims:   claims,
            notBefore: DateTime.UtcNow,
            expires:  DateTime.UtcNow.Add(_jwt.AccessTokenLifetime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}