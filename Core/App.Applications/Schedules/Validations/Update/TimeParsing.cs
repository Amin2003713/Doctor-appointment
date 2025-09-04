using System.Text.RegularExpressions;
using App.Applications.Schedules.Requests.Get;

namespace App.Applications.Schedules.Validations.Update;

public static class TimeParsing
{
    private readonly static Regex HhMm = new(@"^(?:[01]\d|2[0-3]):[0-5]\d$", RegexOptions.Compiled);

    public static bool IsHhMm(string? s)
    {
        return !string.IsNullOrWhiteSpace(s) && HhMm.IsMatch(s!);
    }

    public static bool TryParse(string s, out TimeOnly t)
    {
        if (TimeOnly.TryParseExact(s, "HH:mm", out var r))
        {
            t = r;
            return true;
        }

        t = default;
        return false;
    }

    public static bool FromLessThanTo(TimeRangeDto r)
    {
        return TryParse(r.From, out var f) && TryParse(r.To, out var t) && f < t;
    }

    public static bool Overlaps(TimeRangeDto a, TimeRangeDto b)
    {
        if (!TryParse(a.From, out var af) || !TryParse(a.To, out var at)) return false;
        if (!TryParse(b.From, out var bf) || !TryParse(b.To, out var bt)) return false;

        return af < bt && bf < at;
    }
}