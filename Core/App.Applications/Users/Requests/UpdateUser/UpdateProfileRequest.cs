using MediatR;

namespace App.Applications.Users.Requests.UpdateUser;

public class UpdateProfileRequest : IRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Profile { get; set; }
 
}