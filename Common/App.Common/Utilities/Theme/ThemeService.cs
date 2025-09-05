using MudBlazor;

namespace App.Common.Utilities.Theme;

public class ThemeService : IThemeService
{
    private readonly MudTheme _darkTheme;
    private readonly MudTheme _lightTheme;

    public ThemeService()
    {
        _lightTheme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary                  = "#4CAF50", // Vibrant green for health
                Secondary                = "#3F51B5", // Deep blue for trust
                Tertiary                 = "#F06292", // Coral for warmth
                Background               = "#F5F7FA", // Soft off-white
                Surface                  = "#FFFFFF", // Clean white
                AppbarBackground         = "#3F51B5", // Blue for appbar
                AppbarText               = "#FFFFFF", // White for contrast
                DrawerBackground         = "#FFFFFF", // White drawer
                DrawerText               = "#2E2E2E", // Dark gray text
                TextPrimary              = "#2E2E2E", // Dark gray for readability
                TextSecondary            = "#5A5A5A", // Lighter gray for secondary
                ActionDefault            = "#F06292", // Coral for actions
                ActionDisabled           = "#B0B0B0", // Neutral disabled
                ActionDisabledBackground = "#E5E5E5", // Light disabled background
                Divider                  = "#D1D5DB", // Subtle gray divider
                Success                  = "#2E7D32", // Green for success
                Info                     = "#0288D1", // Blue for info
                Warning                  = "#FBC02D", // Yellow for warning
                Error                    = "#D32F2F"  // Red for error
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto", "Arial", "sans-serif" },
                    FontSize   = "0.875rem"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "2.125rem"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.75rem"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.5rem"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.25rem"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.125rem"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1rem"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "0.875rem"
                },
                Body1 = new Body1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Body2 = new Body2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Caption = new CaptionTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Overline = new OverlineTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                }
            }
        };

        _darkTheme = new MudTheme
        {
            PaletteDark = new PaletteDark
            {
                Primary                  = "#66BB6A", // Lighter green for health
                Secondary                = "#5C6BC0", // Softer blue for trust
                Tertiary                 = "#FF8A80", // Muted coral for warmth
                Background               = "#121212", // Standard dark background
                Surface                  = "#1E1E1E", // Slightly lighter surface
                AppbarBackground         = "#1E1E1E", // Dark gray appbar
                AppbarText               = "#E8ECEF", // Off-white text
                DrawerBackground         = "#1E1E1E", // Dark drawer
                DrawerText               = "#B0BEC5", // Light gray text
                TextPrimary              = "#E8ECEF", // Off-white for readability
                TextSecondary            = "#A0AEC0", // Muted gray for secondary
                ActionDefault            = "#FF8A80", // Coral for actions
                ActionDisabled           = "rgba(255,255,255, 0.3)",
                ActionDisabledBackground = "rgba(255,255,255, 0.08)",
                Divider                  = "rgba(255,255,255, 0.12)",
                Success                  = "#66BB6A", // Green for success
                Info                     = "#4FC3F7", // Light blue for info
                Warning                  = "#FFCA28", // Amber for warning
                Error                    = "#EF5350"  // Red for error
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto", "Arial", "sans-serif" },
                    FontSize   = "0.875rem"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "2.125rem"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.75rem"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.5rem"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.25rem"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.125rem"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1rem"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "0.875rem"
                },
                Body1 = new Body1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Body2 = new Body2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Caption = new CaptionTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Overline = new OverlineTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                }
            }
        };

        CurrentTheme = _darkTheme;
        IsDarkMode   = true;
    }

    public bool IsDarkMode { get; private set; }

    public event EventHandler? ThemeChanged;

    public MudTheme CurrentTheme { get; private set; }

    public void ToggleDarkLightMode(bool isDarkMode, bool refresh = true)
    {
        if (isDarkMode)
            SetDarkTheme(refresh);
        else
            SetLightTheme(refresh);
    }

    public void SetLightTheme(bool refresh)
    {
        CurrentTheme = _lightTheme;
        IsDarkMode   = false;

        if (refresh)
            OnThemeChanged();
    }

    public void SetDarkTheme(bool refresh)
    {
        CurrentTheme = _darkTheme;
        IsDarkMode   = true;

        if (refresh)
            OnThemeChanged();
    }

    public Task OnSystemPreferenceChanged(bool theme)
    {
        ToggleDarkLightMode(theme);
        return Task.CompletedTask;
    }

    protected virtual void OnThemeChanged()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }
}