namespace AppointmentPlanner.Shared.Models;

public class Specialization
{
    public int? DepartmentId { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
    public string Color { get; set; }

    public Specialization() { }

    public Specialization(int? DepartmentId, string Id, string Text, string Color)
    {
        this.DepartmentId = DepartmentId;
        this.Id = Id;
        this.Text = Text;
        this.Color = Color;
    }


    public List<Specialization> GetSpecializationData()
    {
        List<Specialization> data = new List<Specialization>
        {
            new Specialization(1, "generalmedicine", "General Medicine", "#F538B2"),
            new Specialization(2, "neurology",       "Neurology",        "#33C7E8"),
            new Specialization(3, "dermatology",     "Dermatology",      "#916DE4"),
            new Specialization(4, "orthopedics",     "Orthopedics",      "#388CF5"),
            new Specialization(5, "diabetology",     "Diabetology",      "#60F238"),
            new Specialization(6, "cardiology",      "Cardiology",       "#F29438")
        };

        return data;
    }
}