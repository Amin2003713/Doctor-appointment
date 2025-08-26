namespace Api.Endpoints.Dtos.Users;

public record LoginDto(
    string PhoneNumber,      // login via phone
    string Password
);