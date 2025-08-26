namespace Api.Endpoints.Dtos.Users;

public record ResetPasswordDto(
    string PhoneNumber ,
    string Password ,
    string ConfirmPassword
);