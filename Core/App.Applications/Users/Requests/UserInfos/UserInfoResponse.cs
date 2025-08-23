public class UserInfoResponse
{
    public long Id { get; set; } 
    public string PhoneNumber { get; set; } 
    public string? Email { get; set; } 
    public string? FullName { get; set; } 
    public bool IsActive { get; set; } 
    public DateTime CreatedAtUtc { get; set; } 
    public DateTime? LastLoginAtUtc { get; set; } 
    public IEnumerable<string> Roles { get; set; } 
   
}