namespace AppointmentPlanner.Shared.Models;

public class Block
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string RecurrenceRule { get; set; }
    public bool IsAllDay { get; set; }
    public bool IsBlock { get; set; }
    public int[] DoctorId { get; set; }

    public Block() { }

    public Block(int Id, string Name, string StartTime, string EndTime, string RecurrenceRule, bool IsAllDay, bool IsBlock, int[] DoctorId)
    {
        this.Id = Id;
        this.Name = Name;
        this.StartTime = StartTime;
        this.EndTime = EndTime;
        this.RecurrenceRule = RecurrenceRule;
        this.IsAllDay = IsAllDay;
        this.IsBlock = IsBlock;
        this.DoctorId = DoctorId;

    }
}