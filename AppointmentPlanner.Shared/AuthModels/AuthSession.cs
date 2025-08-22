namespace AppointmentPlanner.Shared.AuthModels;

/// <summary>Persisted session (single key in LocalStorage).</summary>
public sealed record AuthSession(
    string   AccessToken ,
    string   RefreshToken ,
    DateTime ExpiresAtUtc ,
    AuthUser User
);