namespace Api.Endpoints.Utilities;

public static class PhoneHelper
{
   
    
    public static string NormalizeUsername(string phoneE164)
    {
        if (string.IsNullOrWhiteSpace(phoneE164))
            return phoneE164;

        
        if (phoneE164.StartsWith("+98"))
            return phoneE164[3..]; 

        if (phoneE164.StartsWith("0098"))
            return phoneE164[4..]; 

        
        if (phoneE164.StartsWith("+"))
            return phoneE164[1..];

        return phoneE164;
    }
}