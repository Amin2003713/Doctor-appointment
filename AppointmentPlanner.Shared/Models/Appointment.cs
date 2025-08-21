namespace AppointmentPlanner.Shared.Models;

public class Appointment
{
    public string Time { get; set; }
    public string Name { get; set; }
    public string DoctorName { get; set; }
    public string Symptoms { get; set; }
    public int DoctorId { get; set; }

    public Appointment() { }

    public Appointment(string Time, string Name, string DoctorName, string Symptoms, int DoctorId)
    {
        this.Time = Time;
        this.Name = Name;
        this.DoctorName = DoctorName;
        this.Symptoms = Symptoms;
        this.DoctorId = DoctorId;
    }
}