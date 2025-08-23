public sealed class JwtOptions
{
    public string Issuer { get; init; } = "yourapp";
    public string Audience { get; init; } = "yourapp";
    public string SigningKey { get; init; } = "super_secret_key_CHANGE_ME_haha_ma-ma-ma_arr_arr";
    public TimeSpan AccessTokenLifetime { get; init; } = TimeSpan.FromDays(1);
}