using System.ComponentModel.DataAnnotations;

namespace AppointmentPlanner.Shared.Models;

public class Patient
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Enter a valid name.")]
    public string Name { get; set; }
    public string Text { get; set; }
    [Required(ErrorMessage = "Select a valid DOB.")]
    public DateTime? DOB { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Enter a valid mobile number.")]
    public string Mobile { get; set; }
    [Required(ErrorMessage = "A valid email address is required.")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; }
    public string Address { get; set; }
    public string Disease { get; set; }
    public string DepartmentName { get; set; }
    [Required]
    public string BloodGroup { get; set; } = "AB +ve";
    public string Gender { get; set; } = "Male";
    public string Symptoms { get; set; }
    public Patient()
    {

    }

    public Patient(int Id, string Name, string Text, DateTime? DOB, string Mobile, string Email, string Address, string Disease, string DepartmentName, string BloodGroup, string Gender, string Symptoms)
    {
        this.Id = Id;
        this.Name = Name;
        this.Text = Text;
        this.DOB = DOB;
        this.Mobile = Mobile;
        this.Email = Email;
        this.Address = Address;
        this.Disease = Disease;
        this.DepartmentName = DepartmentName;
        this.BloodGroup = BloodGroup;
        this.Gender = Gender;
        this.Symptoms = Symptoms;
    }

    public List<Patient> GetPatientsData()
    {
        List<Patient> data = new List<Patient>
        {
            new Patient(1, "Laura", "Laura", new DateTime(1980, 8, 3), "(071) 555-4444", "laura90@mail.com", "507 - 20th Ave. E.\r\nApt. 2A", "Eye Checkup", "GENERAL", "O +ve", "Female", "Sweating, Chills and Shivering"),
            new Patient(2, "Milka", "Milka", new DateTime(2000, 3, 5), "(071) 555-4445", "milka40@sample.com", "908 W. Capital Way", "Bone Fracture", "ORTHOPEDICS", "AB +ve", "Female", "Swelling or bruising over a bone, Pain in the injured area"),
            new Patient(3, "Adams", "Adams", new DateTime(1985, 2, 3), "(071) 555-4454", "adams89@rpy.com", "722 Moss Bay Blvd.", "Eye and Spectactles", "GENERAL", "B +ve", "Male", "Frequent squinting, Eye fatigue or strain"),
            new Patient(4, "Janet", "Janet", new DateTime(2000, 7, 3), "(071) 555-4544", "janet79@rpy.com", "4110 Old Redmond Rd.", "Biological Problem", "GENERAL", "B +ve", "Male", "Physical aches or pain, Memory difficulties or personality change"),
            new Patient(5, "Mercy", "Mercy", new DateTime(2005, 4, 29), "(071) 555-5444", "mercy60@sample.com", "14 Garrett Hill", "Skin Hives", "DERMATOLOGY", "AB -ve", "Female", "outbreak of swollen, pale red bumps or plaques"),
            new Patient(6, "Richa", "Richa", new DateTime(1989, 10, 29), "(206) 555-4444", "richa46@mail.com", "Coventry House\r\nMiner Rd.", "Arm Fracture", "ORTHOPEDICS", "B +ve", "Female", "Swelling, warmth, or redness in the joint"),
            new Patient(7, "Maud Oliver", "Maud Oliver", new DateTime(1989, 10, 29), "(206) 666-4444", "moud46@rpy.com", "Coventry House\r\nMiner Rd.", "Racing heartbeat", "CARDIOLOGY", "B +ve", "Male", "A fluttering in your chest")
        };
        return data;
    }

}