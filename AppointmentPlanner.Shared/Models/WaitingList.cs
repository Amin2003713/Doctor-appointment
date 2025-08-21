namespace AppointmentPlanner.Shared.Models;

public class WaitingList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Disease { get; set; }
    public string DepartmentName { get; set; }
    public string Treatment { get; set; }
    public int DepartmentId { get; set; }
    public int PatientId { get; set; }

    public List<WaitingList> GetWaitingList()
    {

        List<WaitingList> wait = new List<WaitingList>()
        {
            new WaitingList {Id= 1, Name= "Laura", StartTime= new DateTime(2020, 2, 3, 8, 30, 0), EndTime= new DateTime(2020, 2, 3, 9, 30, 0), Disease= "Sudden loss of vision", DepartmentName= "GENERAL", Treatment= "CHECKUP", DepartmentId= 1, PatientId= 1},
            new WaitingList { Id= 2,Name= "Milka", StartTime= new DateTime(2020, 2, 4, 8, 30, 0), EndTime= new DateTime(2020, 2, 4, 10, 30, 0), Disease= "Bone Fracture", DepartmentName= "ORTHOPEDICS", Treatment= "SURGERY", DepartmentId= 4, PatientId= 2 },
            new WaitingList { Id= 3, Name= "Adams", StartTime= new DateTime(2020, 2, 4, 9, 30, 0), EndTime= new DateTime(2020, 2, 4, 10, 30, 0), Disease= "Skin Hives", DepartmentName= "DERMATOLOGY", Treatment= "CHECKUP", DepartmentId= 3, PatientId= 3 },
            new WaitingList { Id= 4, Name= "Janet", StartTime= new DateTime(2020, 2, 3, 11, 0, 0), EndTime= new DateTime(2020, 2, 3, 12, 30, 0), Disease= "Frequent urination", DepartmentName= "DIABETALOGY", Treatment= "DIALOGIS", DepartmentId= 5, PatientId= 4 },
            new WaitingList { Id= 5, Name= "Mercy", StartTime= new DateTime(2020, 2, 3, 11, 0, 0), EndTime= new DateTime(2020, 2, 3, 12, 30, 0), Disease= "Muscle weakness", DepartmentName= "NEUROLOGY", Treatment= "DIAGNOSIS", DepartmentId= 2, PatientId= 5 },
            new WaitingList { Id= 6, Name= "Richa", StartTime= new DateTime(2020, 2, 3, 11, 0, 0), EndTime= new DateTime(2020, 2, 3, 12, 30, 0), Disease= "Shortness of breath", DepartmentName= "CARDIOLOGY", Treatment= "REGULAR CHECKUP", DepartmentId= 6, PatientId= 6 },
            new WaitingList { Id= 7, Name= "Richa", StartTime= new DateTime(2020, 2, 3, 8, 30, 0), EndTime= new DateTime(2020, 2, 3, 9, 30, 0), Disease= "Sudden loss of vision", DepartmentName= "GENERAL", Treatment= "CHECKUP", DepartmentId= 1, PatientId= 6 },
            new WaitingList { Id= 8, Name= "Mercy", StartTime= new DateTime(2020, 8, 4, 8, 30, 0), EndTime= new DateTime(2020, 8, 4, 10, 30, 0), Disease= "Bone Fracture", DepartmentName= "ORTHOPEDICS", Treatment= "SURGERY", DepartmentId= 4, PatientId= 5},
            new WaitingList { Id= 9, Name= "Janet", StartTime= new DateTime(2020, 2, 4, 9, 30, 0), EndTime= new DateTime(2020, 2, 4, 10, 30, 0), Disease= "Skin Hives", DepartmentName= "DERMATOLOGY", Treatment= "CHECKUP", DepartmentId= 3, PatientId= 4 }

        };
        return wait;

    }
}