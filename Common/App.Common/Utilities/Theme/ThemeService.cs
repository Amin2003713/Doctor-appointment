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
            // === LIGHT THEME ===
            _lightTheme = new MudTheme
            {
                PaletteLight = new PaletteLight
                {
                    Primary                  = "#c5aff3", // Greenish mint
                    Secondary                = "#A19397", // Dusty Gray
                    Background               = "#FAFAFA",
                    Surface                  = "#FFFFFF",
                    AppbarBackground         = "#c5aff3",
                    AppbarText               = "#030303",
                    DrawerBackground         = "#FFFFFF",
                    DrawerText               = "#030303",
                    TextPrimary              = "#030303",
                    TextSecondary            = "#555555",
                    Success                  = "#7AF63B",
                    Info                     = "#3B71E4",
                    Warning                  = "#E5A226",
                    Error                    = "#8F8E8D", // Danger
                    Divider                  = "#E0E0E0",
                    ActionDefault            = "#c5aff3",
                    ActionDisabled           = "#BDBDBD",
                    ActionDisabledBackground = "#E0E0E0"
                },
                Typography = BuildTypography()
            };

            // === DARK THEME ===
            _darkTheme = new MudTheme
            {
                PaletteDark = new PaletteDark
                {
                    Primary                  = "#c5aff3", // keep consistent
                    Secondary                = "#A19397",
                    Background               = "#121212",
                    Surface                  = "#1E1E1E",
                    AppbarBackground         = "#030303",
                    AppbarText               = "#FFFFFF",
                    DrawerBackground         = "#1E1E1E",
                    DrawerText               = "#CFD8DC",
                    TextPrimary              = "#FAFAFA",
                    TextSecondary            = "#B0BEC5",
                    Success                  = "#7AF63B",
                    Info                     = "#3B71E4",
                    Warning                  = "#E5A226",
                    Error                    = "#8F8E8D",
                    Divider                  = "rgba(255,255,255,0.1)",
                    ActionDefault            = "#c5aff3",
                    ActionDisabled           = "rgba(255,255,255,0.3)",
                    ActionDisabledBackground = "rgba(255,255,255,0.1)"
                },
                Typography = BuildTypography()
            };

            // default
            _currentTheme = _darkTheme;
            IsDarkMode = true;
        }

        public MudTheme CurrentTheme => _currentTheme;

        public void ToggleDarkLightMode(bool isDarkMode, bool refresh = true)
        {
            if (isDarkMode)
                SetDarkTheme(refresh);
            else
                SetLightTheme(refresh);
        }

        public void SetLightTheme(bool refresh)
        {
            _currentTheme = _lightTheme;
            IsDarkMode = false;
            if (refresh) OnThemeChanged();
        }

        public void SetDarkTheme(bool refresh)
        {
            _currentTheme = _darkTheme;
            IsDarkMode = true;
            if (refresh) OnThemeChanged();
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

        private static Typography BuildTypography() =>
            new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[]
                    {
                        "B Ziba",
                        "Poppins",
                        "Tahoma",
                        "Arial",
                        "Helvetica",
                        "sans-serif"
                    },
                    FontSize = "1rem"
                },
                H1 = new H1Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "2rem" },
                H2 = new H2Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1.75rem" },
                H3 = new H3Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1.5rem" },
                H4 = new H4Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1.25rem" },
                H5 = new H5Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1.125rem" },
                H6 = new H6Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1rem" },
                Button = new ButtonTypography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1rem" },
                Body1 = new Body1Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1.05rem" },
                Body2 = new Body2Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "1rem" },
                Subtitle1 = new Subtitle1Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "0.95rem" },
                Subtitle2 = new Subtitle2Typography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "0.875rem" },
                Caption = new CaptionTypography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "0.8rem" },
                Overline = new OverlineTypography { FontFamily = new[] { "B Ziba", "Poppins" }, FontSize = "0.75rem" },
            };
    }
}
