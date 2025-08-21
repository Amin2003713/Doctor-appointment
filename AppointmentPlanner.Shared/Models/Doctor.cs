using System.ComponentModel.DataAnnotations;

namespace AppointmentPlanner.Shared.Models;

public class Doctor
{
    [Required(ErrorMessage = "Enter a valid name.")]
    public string Name { get; set; }

    public string Gender { get; set; } = "Male";
    public string Text { get; set; }
    public int Id { get; set; }
    public int DepartmentId { get; set; } = 1;
    public string Color { get; set; }
    public string Education { get; set; }
    public string Specialization { get; set; } = "generalmedicine";
    public string Experience { get; set; } = "1+ years";
    public string Designation { get; set; }
    public string DutyTiming { get; set; } = "Shift1";

    [Required(ErrorMessage = "A valid email address is required.")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Enter a valid mobile number.")]
    public string Mobile { get; set; }

    public string Availability { get; set; }
    public string StartHour { get; set; }
    public string EndHour { get; set; }
    public int[] AvailableDays { get; set; }
    public List<WorkDay> WorkDays { get; set; }

    public Doctor() { }

    public Doctor(
        string Name,
        string Gender,
        string Text,
        int Id,
        int DepartmentId,
        string Color,
        string Education,
        string Specialization,
        string Experience,
        string Designation,
        string DutyTiming,
        string Email,
        string Mobile,
        string Availability,
        string StartHour,
        string EndHour,
        int[] AvailableDays,
        List<WorkDay> WorkDays)
    {
        this.Name = Name;
        this.Gender = Gender;
        this.Text = Text;
        this.Id = Id;
        this.DepartmentId = DepartmentId;
        this.Color = Color;
        this.Education = Education;
        this.Specialization = Specialization;
        this.Experience = Experience;
        this.Designation = Designation;
        this.DutyTiming = DutyTiming;
        this.Email = Email;
        this.Mobile = Mobile;
        this.Availability = Availability;
        this.StartHour = StartHour;
        this.EndHour = EndHour;
        this.AvailableDays = AvailableDays;
        this.WorkDays = WorkDays;
    }

}