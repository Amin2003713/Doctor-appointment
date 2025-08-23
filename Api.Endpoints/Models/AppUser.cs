using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser<long>
{
    public string? FullName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
}