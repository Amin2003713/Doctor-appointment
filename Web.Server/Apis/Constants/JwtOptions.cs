public static class JwtOptions
{
    public static string   Issuer              = "Clinic Management API";
    public static string   Audience            = "Clinic Management Client";
    public static string   SigningKey          = "super_secret_key_CHANGE_ME_haha_ma-ma-ma_arr_arr";
    public static TimeSpan AccessTokenLifetime = TimeSpan.FromDays(1);
}