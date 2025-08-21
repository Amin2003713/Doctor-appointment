namespace AppointmentPlanner.Shared.Models;

public class TextValueData
{
    public string Value { get; set; }
    public string Text { get; set; }

    public TextValueData() { }

    public TextValueData(string Value, string Text)
    {
        this.Value = Value;
        this.Text = Text;
    }

    public List<TextValueData> GetStartHours()
    {
        List<TextValueData> data = new List<TextValueData>
        {
            new TextValueData("08:00", "8.00 AM"),
            new TextValueData("9:00",  "9.00 AM"),
            new TextValueData("10:00", "10.00 AM"),
            new TextValueData("11:00", "11.00 AM"),
            new TextValueData("12:00", "12.00 AM")
        };
        return data;
    }
    public List<TextValueData> GetEndHours()
    {
        List<TextValueData> data = new List<TextValueData>
        {
            new TextValueData("16:00", "4.00 PM"),
            new TextValueData("17:00", "5.00 PM"),
            new TextValueData("18:00", "6.00 PM"),
            new TextValueData("19:00", "7.00 PM"),
            new TextValueData("20:00", "8.00 PM"),
            new TextValueData("21:00", "9.00 PM")
        };
        return data;
    }
    public List<TextValueData> GetViews()
    {
        List<TextValueData> data = new List<TextValueData>
        {
            new TextValueData("Day",   "Daily"),
            new TextValueData("Week",  "Weekly"),
            new TextValueData("Month", "Monthly")
        };
        return data;
    }
    public List<TextValueData> GetColorCategory()
    {
        List<TextValueData> data = new List<TextValueData>
        {
            new TextValueData("Departments", "Department Colors"),
            new TextValueData("Doctors",     "Doctors Colors")
        };
        return data;
    }
    public List<TextValueData> GetBloodGroupData()
    {
        List<TextValueData> data = new List<TextValueData>
        {
            new TextValueData("AB +ve", "AB +ve"),
            new TextValueData("A +ve",  "A +ve"),
            new TextValueData("B +ve",  "B +ve"),
            new TextValueData("O +ve",  "O +ve"),
            new TextValueData("AB -ve", "AB -ve"),
            new TextValueData("A -ve",  "A -ve"),
            new TextValueData("B -ve",  "B -ve"),
            new TextValueData("O -ve",  "O -ve")
        };
        return data;
    }
}