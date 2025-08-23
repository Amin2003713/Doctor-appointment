namespace App.Applications.Users.Apis;

public static class ApiRoutes
{
    public const string Base = "/api/User";

    // Auth
    public const string Register          = $"{Base}/register";
    public const string RegisterPatient   = $"{Base}/register/patient";
    public const string RegisterSecretary = $"{Base}/register/secretary";
    public const string ChangeRole        = $"{Base}/change-role";
    public const string Login             = $"{Base}/login";
    public const string ForgotPassword    = $"{Base}/forgot-password";
    public const string Me                = $"{Base}/me";

    // Users
    public const string Users            = $"{Base}/users";
    public const string UsersSecretaries = $"{Base}/users/secretaries";

    // Template routes (must be const for attributes)
    public const string UserByIdTemplate = $"{Base}/user/{{id}}";
    public const string ToggleTemplate   = $"{Base}/toggle";

    // (Optional)
    public const string DebugClaims         = $"{Base}/debug-claims";
}