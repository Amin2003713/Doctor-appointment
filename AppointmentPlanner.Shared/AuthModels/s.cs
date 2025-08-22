#region Models

    public sealed record LoginRequest(string Email , string Password);

    public sealed record RegisterRequest(string Email , string Password , string FullName);

    public sealed record AuthUser(string Id , string Email , string FullName);

    public sealed record AuthTokenResponse(
        string   AccessToken ,
        string   RefreshToken ,
        DateTime ExpiresAtUtc ,
        AuthUser User
    );

    /// <summary>Persisted session (single key in LocalStorage).</summary>
    public sealed record AuthSession(
        string   AccessToken ,
        string   RefreshToken ,
        DateTime ExpiresAtUtc ,
        AuthUser User
    );

#endregion