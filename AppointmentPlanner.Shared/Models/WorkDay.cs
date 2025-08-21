using System.ComponentModel.DataAnnotations;

namespace AppointmentPlanner.Shared.Models;

public class WorkDay
{
    public string Day { get; set; }
    public int Index { get; set; }
    public bool Enable { get; set; }
    public DateTime? WorkStartHour { get; set; }
    public DateTime? WorkEndHour { get; set; }
    [Required(ErrorMessage = "Enter valid Hour")]
    public DateTime? BreakStartHour { get; set; } = new DateTime(2020, 2, 01, 12, 0, 0);
    [Required(ErrorMessage = "Enter valid Hour")]
    public DateTime? BreakEndHour { get; set; } = new DateTime(2020, 2, 01, 13, 0, 0);
    public string State { get; set; }

    public WorkDay() { }

    public WorkDay(string Day, int Index, bool Enable, DateTime? WorkStartHour, DateTime? WorkEndHour, DateTime? BreakStartHour, DateTime? BreakEndHour, string State)
    {
        this.Day = Day;
        this.Index = Index;
        this.Enable = Enable;
        this.WorkStartHour = WorkStartHour;
        this.WorkEndHour = WorkEndHour;
        this.BreakStartHour = BreakStartHour;
        this.BreakEndHour = BreakEndHour;
        this.State = State;
    }
}