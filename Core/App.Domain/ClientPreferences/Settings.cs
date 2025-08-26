namespace App.Domain.ClientPreferences;

public class Settings
{
    public int Id { get; set; }
    public bool IsDarkMode { get; set; }
    public bool IsRTL { get; set; } = true;
    public string LanguageCode { get; set; } = "fa-Ir";
}