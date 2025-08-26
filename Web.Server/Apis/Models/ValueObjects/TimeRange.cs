using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Models.ValueObjects;

[Owned]
public record TimeRange(
    TimeOnly From,
    TimeOnly To
)
{
    public int Minutes => (int)(To.ToTimeSpan() - From.ToTimeSpan()).TotalMinutes;

    public bool Overlaps(TimeRange other)
    {
        return From < other.To && other.From < To;
    }
}