namespace Api.Endpoints.Dtos.Users;

public record LoginDto(
    string PhoneNumber,      
    string Password
);