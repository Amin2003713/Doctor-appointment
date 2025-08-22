#region

using MudBlazor;

#endregion

namespace App.Common.General.SiteSettings;

public static class ApplicationThemesSettings
{
    public static PaletteDark GetPaletteDark => new PaletteDark
    {
        Primary          = "#00796B" , // Persian Green
        Secondary        = "#004D40" , // Dark Persian Green
        Tertiary         = "#009688" , // Teal
        Info             = "#0288D1" , // Light Blue
        Success          = "#26A69A" , // Firoozei
        Warning          = "#FBC02D" , // Amber
        Error            = "#D32F2F" , // Red
        Background       = "#121212" , Surface    = "#1E1E1E" , TextPrimary = "#FFFFFF" , TextSecondary = "#B0BEC5" , AppbarBackground = "#1E1E1E" ,
        DrawerBackground = "#1E1E1E" , DrawerText = "#FFFFFF" , DrawerIcon  = "#FFFFFF" ,
    };


    public static PaletteLight GetPaletteLight => new PaletteLight
    {
        Primary          = "#00796B" , // Persian Green
        Secondary        = "#009688" , // Teal
        Tertiary         = "#4DB6AC" , // Light Teal
        Info             = "#0288D1" , // Light Blue
        Success          = "#26A69A" , // Firoozei
        Warning          = "#FBC02D" , // Amber
        Error            = "#D32F2F" , // Red
        Background       = "#FFFFFF" , Surface    = "#F5F5F5" , TextPrimary = "#212121" , TextSecondary = "#757575" , AppbarBackground = "#FFFFFF" ,
        DrawerBackground = "#FFFFFF" , DrawerText = "#212121" , DrawerIcon  = "#212121" ,
    };

    public static LayoutProperties GetLayoutProperties => new LayoutProperties
    {
        DefaultBorderRadius = "2px" , DrawerMiniWidthLeft = "48px" , DrawerMiniWidthRight = "48px" , DrawerWidthLeft = "200px" , DrawerWidthRight = "200px" ,
        AppbarHeight        = "56px" ,
    };
}