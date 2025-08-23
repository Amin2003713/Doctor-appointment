using MediatR;

namespace App.Applications.Users.Requests.Registers;

public class RegisterRequest : IRequest
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public Stream Profile    { get; set; }
    public string Address    { get; set; }
}