namespace AppointmentPlanner.Shared.Models;

public class Activity
{
    public string Name { get; set; }
    public string Message { get; set; }
    public string Time { get; set; }
    public string Type { get; set; }
    public DateTime ActivityTime { get; set; }

    public Activity() { }

    public Activity(string Name, string Message, string Time, string Type, DateTime ActivityTime)
    {
        this.Name = Name;
        this.Message = Message;
        this.Time = Time;
        this.Type = Type;
        this.ActivityTime = ActivityTime;
    }
    public List<Activity> GetActivityData()
    {
        List<Activity> data = new List<Activity>
        {
            new Activity("Added New Doctor", "Dr.Johnson James, Cardiologist", "5 mins ago", "doctor", new DateTime(2020, 2, 1, 9, 0, 0)),
            new Activity("Added New Appointment", "Laura for General Checkup on 7th March, 2020 @ 8.30 AM with Dr.Molli Cobb, Cardiologist", "5 mins ago", "appointment", new DateTime(2020, 2, 1, 11, 0, 0)),
            new Activity("Added New Patient", "James Richard for Fever and cold", "5 mins ago", "patient", new DateTime(2020, 2, 1, 10, 0, 0)),
            new Activity("Added New Appointment", "Joseph for consultation on 7th Feb, 2020 @ 11.10 AM with Dr.Molli Cobb", "5 mins ago", "appointment", new DateTime(2020, 2, 1, 11, 0, 0))
        };
        return data;
    }
}