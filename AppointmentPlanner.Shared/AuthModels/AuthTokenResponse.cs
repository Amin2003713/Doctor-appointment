namespace AppointmentPlanner.Shared.AuthModels;

public sealed record AuthTokenResponse(
    string   AccessToken ,
    string   RefreshToken ,
    DateTime ExpiresAtUtc ,
    AuthUser User
);