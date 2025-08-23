using MediatR;

namespace App.Applications.Users.Requests.Registers.Patient;

public record RegisterRequest(
    string PhoneNumber, // required (username)
    string Password,    // required
    string FullName,    // required
    string? Email       // optional
) : IRequest;