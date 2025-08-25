using MudBlazor;

namespace App.Common.Utilities.Theme
{
    public class ThemeService : IThemeService
    {
        private readonly MudTheme _lightTheme;
        private readonly MudTheme _darkTheme;

        private MudTheme _currentTheme;
        public bool IsDarkMode { get; private set; }

        public event EventHandler? ThemeChanged;

        public ThemeService()
        {
            // === LIGHT THEME (Persian-inspired) ===
            _lightTheme = new MudTheme
            {
                PaletteLight = new PaletteLight()
                {
                    Primary                  = "#00A693" , // Persian Green
                    Secondary                = "#FE28A2" , // Persian Rose
                    Tertiary                 = "#1C39BB" , // Persian Blue
                    Background               = "#FDFDFD" , // Very light background
                    Surface                  = "#FFFFFF" ,
                    AppbarBackground         = "#1C39BB" , // Persian Blue
                    AppbarText               = "#FFFFFF" ,
                    DrawerBackground         = "#FFFFFF" ,
                    DrawerText               = "#3B3B3B" ,
                    TextPrimary              = "#3B3B3B" ,
                    TextSecondary            = "#5B5B5B" ,
                    ActionDefault            = "#FE28A2" , // Use Persian Rose for hover/focus
                    ActionDisabled           = "#BDBDBD" ,
                    ActionDisabledBackground = "#E0E0E0" ,
                    Divider                  = "#E0E0E0" ,
                    Success                  = "#2E7D32" , // Keep defaults or use your own Persian-inspired success color
                    Info                     = "#2196F3" ,
                    Warning                  = "#FBC02D" ,
                    Error                    = "#CA3433" , // Persian Red
                } ,
                Typography = new Typography
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily = ["B nazanin" , "Tahoma" , "Arial" , "Helvetica" , "sans-serif"] ,
                        FontSize   = "0.875rem" ,
                    } 
                }
            };

            // === DARK THEME (Persian-inspired) ===
            _darkTheme = new MudTheme
            {
                PaletteDark = new PaletteDark
                {
                    Primary                  = "#1565C0" ,                // Ocean Blue
                    Secondary                = "#29B6F6" ,                // Light Cyan
                    Tertiary                 = "#0D47A1" ,                // Deep Navy Blue
                    Background               = "#121212" ,                // Dark Gray (for contrast)
                    Surface                  = "#1A1A1A" ,                // Slightly lighter dark background
                    AppbarBackground         = "#0D47A1" ,                // Deep Blue
                    AppbarText               = "#FFFFFF" ,                // White for contrast
                    DrawerBackground         = "#1A1A1A" ,                // Matches Surface
                    DrawerText               = "#B0BEC5" ,                // Light Gray Text
                    TextPrimary              = "#E3F2FD" ,                // Very Light Blue
                    TextSecondary            = "#90A4AE" ,                // Muted Grayish Blue
                    ActionDefault            = "#29B6F6" ,                // Light Cyan (for buttons)
                    ActionDisabled           = "rgba(255,255,255, 0.3)" , // Muted White
                    ActionDisabledBackground = "rgba(255,255,255, 0.1)" , // Subtle disabled effect
                    Divider                  = "rgba(255,255,255, 0.1)" , // Soft white divider
                    Success                  = "#2E7D32" ,                // Green (for success messages)
                    Info                     = "#0288D1" ,                // Strong Cyan
                    Warning                  = "#F9A825" ,                // Yellowish Orange
                    Error                    = "#D32F2F" ,                  // Dark Red
                } ,
                Typography = new Typography
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily = new[] { "B nazanin" , "Tahoma" , "Arial" , "Helvetica" , "sans-serif" } ,
                        FontSize   = "0.875rem"
                    } 
                }
            };

            // Set default to Light theme
            _currentTheme = _darkTheme;
            IsDarkMode    = true;
        }

        public MudTheme CurrentTheme => _currentTheme;

        public void ToggleDarkLightMode(bool isDarkMode , bool refresh = true)
        {
            if (isDarkMode)
                SetDarkTheme(refresh);
            else
                SetLightTheme(refresh);
        }

        public void SetLightTheme(bool refresh)
        {
            _currentTheme = _lightTheme;
            IsDarkMode    = false;
            if (refresh)
                OnThemeChanged();
        }

        public void SetDarkTheme(bool refresh)
        {
            _currentTheme = _darkTheme;
            IsDarkMode    = true;
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
            ThemeChanged?.Invoke(this , EventArgs.Empty);
        }
    }
}