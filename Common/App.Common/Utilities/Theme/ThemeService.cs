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
                Primary                  = "#00A693" , 
                Secondary                = "#FE28A2" , 
                Tertiary                 = "#1C39BB" , 
                Background               = "#FDFDFD" , 
                Surface                  = "#FFFFFF" ,
                AppbarBackground         = "#1C39BB" , 
                AppbarText               = "#FFFFFF" ,
                DrawerBackground         = "#FFFFFF" ,
                DrawerText               = "#3B3B3B" ,
                TextPrimary              = "#3B3B3B" ,
                TextSecondary            = "#5B5B5B" ,
                ActionDefault            = "#FE28A2" , 
                ActionDisabled           = "#BDBDBD" ,
                ActionDisabledBackground = "#E0E0E0" ,
                Divider                  = "#E0E0E0" ,
                Success                  = "#2E7D32" , 
                Info                     = "#2196F3" ,
                Warning                  = "#FBC02D" ,
                Error                    = "#CA3433"  
            } ,
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["B taha" , "Tahoma" , "Arial" , "Helvetica" , "sans-serif"] ,
                    FontSize   = "0.875rem"
                } ,
                H1        = new H1Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "2.125rem"
                } ,
                H2        = new H2Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.75rem"
                } ,
                H3        = new H3Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.5rem"
                } ,
                H4        = new H4Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.25rem"
                } ,
                H5        = new H5Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.125rem"
                } ,
                H6        = new H6Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1rem"
                } ,
                Button    = new ButtonTypography
                {
                    FontFamily    = ["B taha"] ,
                    FontSize = "0.875rem"
                } ,
                Body1     = new Body1Typography
                {
                    FontFamily     = ["B taha"]
                } ,
                Body2     = new Body2Typography
                {
                    FontFamily     = ["B taha"]
                } ,
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = ["B taha"]
                } ,
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = ["B taha"]
                } ,
                Caption   = new CaptionTypography
                {
                    FontFamily   = ["B taha"]
                } ,
                Overline  = new OverlineTypography
                {
                    FontFamily  = ["B taha"]
                }
            }
        };

        
        _darkTheme = new MudTheme
        {
            PaletteDark = new PaletteDark
            {
                Primary                  = "#1565C0" ,                
                Secondary                = "#29B6F6" ,                
                Tertiary                 = "#0D47A1" ,                
                Background               = "#121212" ,                
                Surface                  = "#1A1A1A" ,                
                AppbarBackground         = "#0D47A1" ,                
                AppbarText               = "#FFFFFF" ,                
                DrawerBackground         = "#1A1A1A" ,                
                DrawerText               = "#B0BEC5" ,                
                TextPrimary              = "#E3F2FD" ,                
                TextSecondary            = "#90A4AE" ,                
                ActionDefault            = "#29B6F6" ,                
                ActionDisabled           = "rgba(255,255,255, 0.3)" , 
                ActionDisabledBackground = "rgba(255,255,255, 0.1)" , 
                Divider                  = "rgba(255,255,255, 0.1)" , 
                Success                  = "#2E7D32" ,                
                Info                     = "#0288D1" ,                
                Warning                  = "#F9A825" ,                
                Error                    = "#D32F2F"                  
            } ,
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[]
                    {
                        "B taha" ,
                        "Tahoma" ,
                        "Arial" ,
                        "Helvetica" ,
                        "sans-serif"
                    } ,
                    FontSize   = "0.875rem"
                } ,
                H1        = new H1Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "2.125rem"
                } ,
                H2        = new H2Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.75rem"
                } ,
                H3        = new H3Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.5rem"
                } ,
                H4        = new H4Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.25rem"
                } ,
                H5        = new H5Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1.125rem"
                } ,
                H6        = new H6Typography
                {
                    FontFamily        = ["B taha"] ,
                    FontSize = "1rem"
                } ,
                Button    = new ButtonTypography
                {
                    FontFamily    = ["B taha"] ,
                    FontSize = "0.875rem"
                } ,
                Body1     = new Body1Typography
                {
                    FontFamily     = ["B taha"]
                } ,
                Body2     = new Body2Typography
                {
                    FontFamily     = ["B taha"]
                } ,
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = ["B taha"]
                } ,
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = ["B taha"]
                } ,
                Caption   = new CaptionTypography
                {
                    FontFamily   = ["B taha"]
                } ,
                Overline  = new OverlineTypography
                {
                    FontFamily  = ["B taha"]
                }
            }
        };

        
        CurrentTheme = _darkTheme;
        IsDarkMode    = true;
    }

    public bool IsDarkMode { get; private set; }

    public event EventHandler? ThemeChanged;

    public MudTheme CurrentTheme { get; private set; }

    public void ToggleDarkLightMode(bool isDarkMode , bool refresh = true)
    {
        if (isDarkMode)
            SetDarkTheme(refresh);
        else
            SetLightTheme(refresh);
    }

    public void SetLightTheme(bool refresh)
    {
        CurrentTheme = _lightTheme;
        IsDarkMode    = false;
        if (refresh)
            OnThemeChanged();
    }

    public void SetDarkTheme(bool refresh)
    {
        CurrentTheme = _darkTheme;
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