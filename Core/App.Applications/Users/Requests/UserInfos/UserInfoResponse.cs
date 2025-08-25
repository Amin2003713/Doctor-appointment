namespace App.Applications.Users.Requests.UserInfos;

public class UserInfoResponse
{
    public long Id { get; set; }
    public string Username { get; set; } = default!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? Profile { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}