using Microsoft.AspNetCore.Identity;

namespace AppointmentPlanner.Shared.Models.Users;

public class User : IdentityUser<long>
{
    public string FullName { get; set; } = "";
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}