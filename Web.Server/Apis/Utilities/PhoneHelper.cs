namespace Api.Endpoints.Utilities;

public static class PhoneHelper
{
   
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