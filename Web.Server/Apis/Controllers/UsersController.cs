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
    IWebHostEnvironment env
) : ControllerBase
{
// PUT api/User/profile
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();

        // Update allowed fields
        if (dto.FirstName is not null) user.FirstName = dto.FirstName.Trim();
        if (dto.LastName  is not null) user.LastName  = dto.LastName.Trim();
        if (dto.Address   is not null) user.Address   = dto.Address.Trim();
        if (dto.Profile   is not null) user.Profile   = dto.Profile.Trim();

        // Optional: allow email change (add confirmation flow if needed)
        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email.Trim();

        // Keep FullName in sync
        user.FullName = BuildFullName(user.FirstName, user.LastName);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }


    [HttpPost("profile/avatar")]
    [AllowAnonymous]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file uploaded.");

        // Validate content type
        if (!IsAllowedImageContentType(file.ContentType))
            return BadRequest("Only PNG, JPEG or WebP images are allowed.");

        // Validate extension
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!IsAllowedImageExtension(ext))
            return BadRequest("Only .png, .jpg/.jpeg, or .webp files are allowed.");

        var id      = Guid.NewGuid().ToString();
        var root    = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        var userDir = Path.Combine(root, "uploads", "profiles", id);
        Directory.CreateDirectory(userDir);

        // Unique file name
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(userDir, fileName);

        // OPTIONAL: purge previous avatar files to keep only one
        TryDeleteExistingAvatarFiles(userDir);

        // Save file
        await using (var stream = System.IO.File.Create(fullPath))
            await file.CopyToAsync(stream);

        // Persist relative/public URL
        var publicUrl = $"/uploads/profiles/{id}/{fileName}";

        return Ok(publicUrl);
    }

    private static bool IsAllowedImageContentType(string? contentType)
    {
        // Common browser values
        return contentType is "image/png" or "image/jpeg" or "image/webp";
    }

    private static bool IsAllowedImageExtension(string ext)
    {
        return ext is ".png" or ".jpg" or ".jpeg" or ".webp";
    }

    private static void TryDeleteExistingAvatarFiles(string userDir)
    {
        try
        {
            if (!Directory.Exists(userDir)) return;

            var files = Directory.GetFiles(userDir);
            foreach (var f in files) System.IO.File.Delete(f);
        }
        catch
        {
            /* ignore */
        }
    }

    // ---------------------------
    // Register (self) => Patient
    // ---------------------------
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null) return BadRequest("Phone number already registered.");

        var user = new AppUser
        {
            UserName          = username,
            NormalizedUserName = username,
            PhoneNumber       = e164,
            Email             = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
            FirstName         = dto.FirstName?.Trim(),
            LastName          = dto.LastName?.Trim(),
            Profile           = dto.Profile,
            Address           = dto.Address,
            FullName          = BuildFullName(dto.FirstName, dto.LastName),
            CreatedAtUtc      = DateTime.UtcNow,
            IsActive          = true
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        await EnsureRolesExist();
        await userManager.AddToRoleAsync(user, "Patient");

        return Ok(new UserDetailDto(
            user.Id,
            user.PhoneNumber ?? user.UserName ?? "",
            user.Email,
            user.FirstName,
            user.LastName,
            user.FullName,
            user.Profile,
            user.Address,
            user.IsActive,
            user.CreatedAtUtc,
            user.LastLoginAtUtc,
            Roles: new[]
            {
                "Patient"
            }
        ));
    }

    // ---------------------------
    // Doctor/Secretary create Patient
    // ---------------------------
    [HttpPost("register/patient")]
    [Authorize(Roles = "Doctor,Secretary")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterDto dto)
    {
        var (ok, msg, user) = await RegisterWithRole(dto, "Patient");
        return ok
            ? Ok(ToDetail(user!,
                new[]
                {
                    "Patient"
                }))
            : BadRequest(msg);
    }

    // ---------------------------
    // Doctor create Secretary
    // ---------------------------
    [HttpPost("register/secretary")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> RegisterSecretary([FromBody] RegisterDto dto)
    {
        var (ok, msg, user) = await RegisterWithRole(dto, "Secretary");
        return ok
            ? Ok(ToDetail(user!,
                new[]
                {
                    "Secretary"
                }))
            : BadRequest(msg);
    }

    // ---------------------------
    // Change role (Doctor only)
    // ---------------------------
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

        if (!long.TryParse(dto.UserId, out _))
            return BadRequest("Invalid user id");

        var user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null) return NotFound("User not found");

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRoleAsync(user, dto.NewRole);

        var roles = await userManager.GetRolesAsync(user);
        return Ok(ToDetail(user, roles));
    }

    // ---------------------------
    // Users list (Doctor only)
    // ---------------------------
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
                (u.FullName     != null && u.FullName.Contains(search)) ||
                (u.FirstName    != null && u.FirstName.Contains(search)) ||
                (u.LastName     != null && u.LastName.Contains(search)) ||
                (u.Address      != null && u.Address.Contains(search)) ||
                (u.Email        != null && u.Email.Contains(search)) ||
                (u.PhoneNumber  != null && u.PhoneNumber.Contains(search)) ||
                (u.UserName     != null && u.UserName.Contains(search)));
        }

        var total = q.LongCount(); // use LongCountAsync with EF
        var users = q.OrderByDescending(u => u.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var list = new List<UserListItemDto>(users.Count);

        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            list.Add(new UserListItemDto(
                u.Id,
                u.PhoneNumber ?? u.UserName ?? "",
                u.Email,
                u.FirstName,
                u.LastName,
                u.FullName,
                u.Profile,
                u.Address,
                u.IsActive,
                u.CreatedAtUtc,
                u.LastLoginAtUtc,
                roles
            ));
        }

        return Ok(new PagedResult<UserListItemDto>(list, page, pageSize, total));
    }

    // ---------------------------
    // Secretaries list (Doctor only)
    // ---------------------------
    [HttpGet("users/secretaries")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<PagedResult<UserListItemDto>>> GetSecretaries(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        if (page <= 0) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;

        var secretaries = await userManager.GetUsersInRoleAsync("Secretary");
        var filtered    = secretaries.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            filtered = filtered.Where(u =>
                (u.FullName     != null && u.FullName.Contains(search)) ||
                (u.FirstName    != null && u.FirstName.Contains(search)) ||
                (u.LastName     != null && u.LastName.Contains(search)) ||
                (u.Address      != null && u.Address.Contains(search)) ||
                (u.Email        != null && u.Email.Contains(search)) ||
                (u.PhoneNumber  != null && u.PhoneNumber.Contains(search)) ||
                (u.UserName     != null && u.UserName.Contains(search)));
        }

        var total = filtered.LongCount();
        var pageItems = filtered.OrderByDescending(u => u.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var list = new List<UserListItemDto>(pageItems.Count);

        foreach (var u in pageItems)
        {
            var roles = await userManager.GetRolesAsync(u);
            list.Add(new UserListItemDto(
                u.Id,
                u.PhoneNumber ?? u.UserName ?? "",
                u.Email,
                u.FirstName,
                u.LastName,
                u.FullName,
                u.Profile,
                u.Address,
                u.IsActive,
                u.CreatedAtUtc,
                u.LastLoginAtUtc,
                roles
            ));
        }

        return Ok(new PagedResult<UserListItemDto>(list, page, pageSize, total));
    }

    // ---------------------------
    // Single user (Doctor only)
    // ---------------------------
    [HttpGet("user/{id:long}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<UserDetailDto>> GetUser(long id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound("User not found");

        var roles = await userManager.GetRolesAsync(user);
        return Ok(ToDetail(user, roles));
    }

    // ---------------------------
    // Login
    // ---------------------------
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

        var token = await GenerateJwtToken(user);


        await userManager.UpdateAsync(user);

        return Ok(new LoginResponseDto
        {
            Token     = token,
        });
    }

    // ---------------------------
    // Forgot password (reset)
    // ---------------------------
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordDto dto)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var user = await userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("User not found");

        var token  = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    // ---------------------------
    // Me
    // ---------------------------
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserInfoDto>> Me()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await userManager.GetRolesAsync(user);
        return Ok(new UserInfoDto(
            user.Id,
            user.UserName! ,
            user.Email,
            user.FirstName,
            user.LastName,
            user.FullName ?? $"{user.FirstName} {user.LastName}".Trim(),
            user.Profile,
            user.Address,
            user.IsActive,
            user.CreatedAtUtc,
            user.LastLoginAtUtc,
            roles
        ));
    }

    // ---------------------------
    // Toggle active (Doctor only)
    // ---------------------------
    [HttpPost("toggle")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> ToggleUser([FromBody] ToggleUserDto dto)
    {
        if (!long.TryParse(dto.UserId, out _)) return BadRequest("Invalid user id");

        var user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null) return NotFound("User not found");

        user.IsActive = !user.IsActive;
        await userManager.UpdateAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        return Ok(ToDetail(user, roles));
    }

    // ---------------------------
    // Helpers
    // ---------------------------
    private static string? BuildFullName(string? first, string? last)
        => string.Join(" ",
            new[]
            {
                first,
                last
            }.Where(s => !string.IsNullOrWhiteSpace(s)));

    private async Task<(bool ok, string? error, AppUser? user)> RegisterWithRole(RegisterDto dto, string role)
    {
        var e164     = PhoneHelper.NormalizeToE164Guess(dto.PhoneNumber);
        var username = PhoneHelper.NormalizeUsername(e164);

        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null) return (false, "Phone number already registered.", null);

        var user = new AppUser
        {
            UserName          = username,
            NormalizedUserName = username,
            PhoneNumber       = e164,
            Email             = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
            FirstName         = dto.FirstName?.Trim(),
            LastName          = dto.LastName?.Trim(),
            Profile           = dto.Profile,
            Address           = dto.Address,
            FullName          = BuildFullName(dto.FirstName, dto.LastName),
            CreatedAtUtc      = DateTime.UtcNow,
            IsActive          = true
        };

        var create = await userManager.CreateAsync(user, dto.Password);
        if (!create.Succeeded) return (false, string.Join("; ", create.Errors.Select(e => e.Description)), null);

        await EnsureRolesExist();
        await userManager.AddToRoleAsync(user, role);
        return (true, null, user);
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
            new(ClaimTypes.Name,  user.UserName ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new("first_name", user.FirstName ?? string.Empty),
            new("last_name",  user.LastName  ?? string.Empty),
            new("profile",    user.Profile   ?? string.Empty),
            new("address",    user.Address   ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtOptions.Issuer,
            audience: JwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(JwtOptions.AccessTokenLifetime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserDetailDto ToDetail(AppUser u, IEnumerable<string> roles)
        => new(
            u.Id,
            u.PhoneNumber ?? u.UserName ?? "",
            u.Email,
            u.FirstName,
            u.LastName,
            u.FullName ?? BuildFullName(u.FirstName, u.LastName),
            u.Profile,
            u.Address,
            u.IsActive,
            u.CreatedAtUtc,
            u.LastLoginAtUtc,
            roles.ToArray()
        );
}