using App.Common.Utilities.LifeTime;
using MudBlazor;

namespace App.Common.Utilities.Theme;

public interface IThemeService : IScopedDependency
{
    
    
    
    MudTheme CurrentTheme { get; }

    
    
    /// </summary>
    bool IsDarkMode { get; }

    /// <summary>
    ///     Event raised whenever the theme changes.
    /// </summary>
    event EventHandler ThemeChanged;

    /// <summary>
    ///     Switch between Dark and Light modes.
    /// </summary>
    /// <param name="isDarkMode"></param>
    /// <param name="refresh"></param>
    void ToggleDarkLightMode(bool isDarkMode , bool refresh = true);

    /// <summary>
    ///     Optionally, set the theme to Light or Dark specifically.
    /// </summary>
    void SetLightTheme(bool refresh);

    void SetDarkTheme(bool refresh);
    Task OnSystemPreferenceChanged(bool isDarkMode);
}