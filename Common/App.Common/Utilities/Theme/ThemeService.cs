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
                PaletteLight = new PaletteLight
                {
                    Primary                  = "#1976D2", // Medical Blue
                    Secondary                = "#4CAF50", // Healing Green
                    Tertiary                 = "#FDD835", // Warm Yellow
                    Background               = "#FAFAFA", // Light Grayish
                    Surface                  = "#FFFFFF",
                    AppbarBackground         = "#1976D2",
                    AppbarText               = "#FFFFFF",
                    DrawerBackground         = "#FFFFFF",
                    DrawerText               = "#333333",
                    TextPrimary              = "#212121",
                    TextSecondary            = "#555555",
                    ActionDefault            = "#1976D2",
                    ActionDisabled           = "#BDBDBD",
                    ActionDisabledBackground = "#E0E0E0",
                    Divider                  = "#E0E0E0",
                    Success                  = "#388E3C",
                    Info                     = "#0288D1",
                    Warning                  = "#FFA000",
                    Error                    = "#D32F2F"
                } ,
                Typography = new Typography
                {
                    Default    = new DefaultTypography
                    {
                        FontFamily = ["B Ziba", "Tahoma", "Arial", "Helvetica", "sans-serif"],
                        FontSize = "1rem"
                    },
                    H1         = new H1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "2rem"
                    },
                    H2         = new H2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.75rem"
                    },
                    H3         = new H3Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.5rem"
                    },
                    H4         = new H4Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.25rem"
                    },
                    H5         = new H5Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.125rem"
                    },
                    H6         = new H6Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1rem"
                    },
                    Button     = new ButtonTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Body1      = new Body1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1rem"
                    },
                    Body2      = new Body2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Subtitle1  = new Subtitle1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Subtitle2  = new Subtitle2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.875rem"
                    },
                    Caption    = new CaptionTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.75rem"
                    },
                    Overline   = new OverlineTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.75rem"
                    }
                }
            };

            // === DARK THEME (Persian-inspired) ===
            _darkTheme = new MudTheme
            {
                PaletteDark = new PaletteDark
                {
                    Primary                  = "#90CAF9", // Light Blue
                    Secondary                = "#A5D6A7", // Soft Green
                    Tertiary                 = "#FFF59D", // Warm Yellow
                    Background               = "#121212",
                    Surface                  = "#1E1E1E",
                    AppbarBackground         = "#1E88E5",
                    AppbarText               = "#FFFFFF",
                    DrawerBackground         = "#1E1E1E",
                    DrawerText               = "#CFD8DC",
                    TextPrimary              = "#E0F7FA",
                    TextSecondary            = "#B0BEC5",
                    ActionDefault            = "#90CAF9",
                    ActionDisabled           = "rgba(255,255,255,0.3)",
                    ActionDisabledBackground = "rgba(255,255,255,0.1)",
                    Divider                  = "rgba(255,255,255,0.1)",
                    Success                  = "#66BB6A",
                    Info                     = "#4FC3F7",
                    Warning                  = "#FFB300",
                    Error                    = "#EF5350"
                },
                Typography = new Typography
                {
                    Default    = new DefaultTypography
                    {
                        FontFamily = ["B Ziba", "Tahoma", "Arial", "Helvetica", "sans-serif"],
                        FontSize = "1rem"
                    },
                    H1         = new H1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "2rem"
                    },
                    H2         = new H2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.75rem"
                    },
                    H3         = new H3Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.5rem"
                    },
                    H4         = new H4Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.25rem"
                    },
                    H5         = new H5Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1.125rem"
                    },
                    H6         = new H6Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1rem"
                    },
                    Button     = new ButtonTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Body1      = new Body1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "1rem"
                    },
                    Body2      = new Body2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Subtitle1  = new Subtitle1Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.95rem"
                    },
                    Subtitle2  = new Subtitle2Typography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.875rem"
                    },
                    Caption    = new CaptionTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.75rem"
                    },
                    Overline   = new OverlineTypography
                    {
                        FontFamily = ["B Ziba"],
                        FontSize = "0.75rem"
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