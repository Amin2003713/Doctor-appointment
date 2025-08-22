namespace AppointmentPlanner.Shared.Dtos;

public class MinimalUser
{
    public long Id { get; set; }
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
}