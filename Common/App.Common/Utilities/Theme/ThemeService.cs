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
                        FontFamily = ["B taha" , "Tahoma" , "Arial" , "Helvetica" , "sans-serif"] ,
                        FontSize   = "0.875rem" ,
                    } ,
                    H1        = new H1Typography() { FontFamily        = ["B taha"] , FontSize = "2.125rem" } ,
                    H2        = new H2Typography() { FontFamily        = ["B taha"] , FontSize = "1.75rem" } ,
                    H3        = new H3Typography() { FontFamily        = ["B taha"] , FontSize = "1.5rem" } ,
                    H4        = new H4Typography() { FontFamily        = ["B taha"] , FontSize = "1.25rem" } ,
                    H5        = new H5Typography() { FontFamily        = ["B taha"] , FontSize = "1.125rem" } ,
                    H6        = new H6Typography() { FontFamily        = ["B taha"] , FontSize = "1rem" } ,
                    Button    = new ButtonTypography() { FontFamily    = ["B taha"] , FontSize = "0.875rem" } ,
                    Body1     = new Body1Typography() { FontFamily     = ["B taha"] } ,
                    Body2     = new Body2Typography() { FontFamily     = ["B taha"] } ,
                    Subtitle1 = new Subtitle1Typography() { FontFamily = ["B taha"] } ,
                    Subtitle2 = new Subtitle2Typography() { FontFamily = ["B taha"] } ,
                    Caption   = new CaptionTypography() { FontFamily   = ["B taha"] } ,
                    Overline  = new OverlineTypography() { FontFamily  = ["B taha"] }
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
                        FontFamily = new[] { "B taha" , "Tahoma" , "Arial" , "Helvetica" , "sans-serif" } ,
                        FontSize   = "0.875rem"
                    } ,
                    H1        = new H1Typography() { FontFamily        = ["B taha"] , FontSize = "2.125rem" } ,
                    H2        = new H2Typography() { FontFamily        = ["B taha"] , FontSize = "1.75rem" } ,
                    H3        = new H3Typography() { FontFamily        = ["B taha"] , FontSize = "1.5rem" } ,
                    H4        = new H4Typography() { FontFamily        = ["B taha"] , FontSize = "1.25rem" } ,
                    H5        = new H5Typography() { FontFamily        = ["B taha"] , FontSize = "1.125rem" } ,
                    H6        = new H6Typography() { FontFamily        = ["B taha"] , FontSize = "1rem" } ,
                    Button    = new ButtonTypography() { FontFamily    = ["B taha"] , FontSize = "0.875rem" } ,
                    Body1     = new Body1Typography() { FontFamily     = ["B taha"] } ,
                    Body2     = new Body2Typography() { FontFamily     = ["B taha"] } ,
                    Subtitle1 = new Subtitle1Typography() { FontFamily = ["B taha"] } ,
                    Subtitle2 = new Subtitle2Typography() { FontFamily = ["B taha"] } ,
                    Caption   = new CaptionTypography() { FontFamily   = ["B taha"] } ,
                    Overline  = new OverlineTypography() { FontFamily  = ["B taha"] }
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