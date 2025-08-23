namespace App.Applications.Users.Requests.UpdateUser;

public record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string? Address,
    string? Email      // optional: allow email change if you want
);