namespace AppointmentPlanner.Shared.Models;

public class CalendarSetting
{
    public string BookingColor { get; set; }
    public Calendar Calendar { get; set; }
    public string CurrentView { get; set; }
    public int Interval { get; set; } = 60;
    public int FirstDayOfWeek { get; set; } = 1;
}