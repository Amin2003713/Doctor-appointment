namespace AppointmentPlanner.Shared.Models;

public class NavigationMenu
{
    public string Text { get; set; }
    public string Value { get; set; } = "dashboard";
    public string Icon { get; set; }

    public NavigationMenu() { }

    public NavigationMenu(string Text, string Value, string Icon)
    {
        this.Text = Text;
        this.Value = Value;
        this.Icon = Icon;
    }
    public List<NavigationMenu> GetNavigationMenuItems()
    {
        List<NavigationMenu> data = new List<NavigationMenu>
        {
            new NavigationMenu("Dashboard",  "dashboard",  "icon-dashboard"),
            new NavigationMenu("Schedule",   "schedule",   "icon-schedule"),
            new NavigationMenu("Doctors",    "doctors",    "icon-doctors"),
            new NavigationMenu("Patients",   "patients",   "icon-patients"),
            new NavigationMenu("Preference", "preference", "icon-preference"),
            new NavigationMenu("About",      "about",      "icon-about")
        };
        return data;
    }
}