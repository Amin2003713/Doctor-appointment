namespace AppointmentPlanner.Shared.Models;

public class TemplateArgs
{
    public string ElementType { get; set; }
    public DateTime? Date { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Disease { get; set; }
    public string DepartmentName { get; set; }
    public int DepartmentId { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string Symptoms { get; set; }
}