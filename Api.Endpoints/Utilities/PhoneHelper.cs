public static class PhoneHelper
{
    // Example: turns "0098 912-345-6789" or "09123456789" into "+989123456789"
    public static string NormalizeToE164Guess(string raw, string defaultCountryCode = "+98")
    {
        if (string.IsNullOrWhiteSpace(raw)) return raw;

        var digits = new string(raw.Where(char.IsDigit).ToArray());

        // Handle leading 00 (international)
        if (digits.StartsWith("00"))
            return "+" + digits[2..];

        // Handle local 0X... -> defaultCountryCode + X...
        if (digits.StartsWith("0"))
            return defaultCountryCode + digits[1..];

        // Already looks like country+subscriber without plus
        if (!digits.StartsWith("0"))
            return digits.StartsWith("9") && defaultCountryCode == "+98"
                ? defaultCountryCode + digits
                : "+" + digits;

        return "+" + digits;
    }

    public static string NormalizeUsername(string phoneE164)
        => phoneE164?.Trim().ToUpperInvariant()!;
}