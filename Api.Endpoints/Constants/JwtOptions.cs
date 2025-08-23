public static class JwtOptions
{
    public static string   Issuer              = "yourapp";
    public static string   Audience            = "yourapp";
    public static string   SigningKey          = "super_secret_key_CHANGE_ME_haha_ma-ma-ma_arr_arr";
    public static TimeSpan AccessTokenLifetime = TimeSpan.FromDays(1);
}