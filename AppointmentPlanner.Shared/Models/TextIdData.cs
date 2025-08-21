namespace AppointmentPlanner.Shared.Models;

public class TextIdData
{
    public string Id { get; set; }
    public string Text { get; set; }

    public TextIdData() { }

    public TextIdData(string Id, string Text)
    {
        this.Id = Id;
        this.Text = Text;
    }

    public List<TextIdData> ExperienceData()
    {
        List<TextIdData> data = new List<TextIdData>
        {
            new TextIdData("1+ years",  "1+ years"),
            new TextIdData("2+ years",  "2+ years"),
            new TextIdData("5+ years",  "5+ years"),
            new TextIdData("10+ years", "10+ years"),
            new TextIdData("15+ years", "15+ years"),
            new TextIdData("20+ years", "20+ years")
        };
        return data;
    }
    public List<TextIdData> DutyTimingsData()
    {
        List<TextIdData> data = new List<TextIdData>
        {
            new TextIdData("Shift1", "08:00 AM - 5:00 PM"),
            new TextIdData("Shift2", "10:00 AM - 7:00 PM"),
            new TextIdData("Shift3", "12:00 PM - 9:00 PM")
        };
        return data;
    }
}