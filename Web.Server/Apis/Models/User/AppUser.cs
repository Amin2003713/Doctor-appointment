using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Models.User;

public class AppUser : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Profile { get; set; }
    public string Address { get; set; }
    public string? FullName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
}