namespace App.Domain.ValueObjects;

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