public static class PhoneHelper
{
    // Convert to E.164 form (keeps +98 etc for PhoneNumber)
    public static string NormalizeToE164Guess(string raw, string defaultCountryCode = "+98")
    {
        if (string.IsNullOrWhiteSpace(raw)) return raw;

        var digits = new string(raw.Where(char.IsDigit).ToArray());

        // Handle leading 00 -> +
        if (digits.StartsWith("00"))
            return "+" + digits[2..];

        // Handle local 0X... -> prepend country code
        if (digits.StartsWith("0"))
            return defaultCountryCode + digits[1..];

        // Already starts with country code digits (assume)
        if (!digits.StartsWith("0"))
            return "+" + digits;

        return "+" + digits;
    }

    // Strip +98 / 0098 and keep just the local part for Username
    public static string NormalizeUsername(string phoneE164)
    {
        if (string.IsNullOrWhiteSpace(phoneE164))
            return phoneE164;

        // Drop +98 or 0098 from start if present
        if (phoneE164.StartsWith("+98"))
            return phoneE164[3..]; // e.g. +98912... -> 912...

        if (phoneE164.StartsWith("0098"))
            return phoneE164[4..]; // e.g. 0098912... -> 912...

        // Drop leading +
        if (phoneE164.StartsWith("+"))
            return phoneE164[1..];

        return phoneE164;
    }

}