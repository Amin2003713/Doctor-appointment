namespace AppointmentPlanner.Shared.Models;

public class TextValueNumericData
{
    public int Value { get; set; }
    public string Text { get; set; }

    public TextValueNumericData() { }

    public TextValueNumericData(int Value, string Text)
    {
        this.Value = Value;
        this.Text = Text;
    }

    public List<TextValueNumericData> GetTimeSlot()
    {
        List<TextValueNumericData> data = new List<TextValueNumericData>
        {
            new TextValueNumericData(10,  "10 mins"),
            new TextValueNumericData(20,  "20 mins"),
            new TextValueNumericData(30,  "30 mins"),
            new TextValueNumericData(60,  "60 mins"),
            new TextValueNumericData(120, "120 mins")
        };
        return data;
    }

    public List<TextValueNumericData> GetDayOfWeekList()
    {
        List<TextValueNumericData> data = new List<TextValueNumericData>
        {
            new TextValueNumericData(0, "Sunday"),
            new TextValueNumericData(1, "Monday"),
            new TextValueNumericData(2, "Tuesday"),
            new TextValueNumericData(3, "Wednesday"),
            new TextValueNumericData(4, "Thursday"),
            new TextValueNumericData(5, "Friday"),
            new TextValueNumericData(6, "Saturday")
        };
        return data;
    }
}