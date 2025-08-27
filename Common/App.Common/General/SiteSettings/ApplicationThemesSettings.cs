#region

    using MudBlazor;

#endregion

    namespace App.Common.General.SiteSettings;

    public static class ApplicationThemesSettings
    {
        public static PaletteDark GetPaletteDark => new()
        {
            Primary          = "#00796B" , 
            Secondary        = "#004D40" , 
            Tertiary         = "#009688" , 
            Info             = "#0288D1" , 
            Success          = "#26A69A" , 
            Warning          = "#FBC02D" , 
            Error            = "#D32F2F" , 
            Background       = "#121212" ,
            Surface    = "#1E1E1E" ,
            TextPrimary = "#FFFFFF" ,
            TextSecondary = "#B0BEC5" ,
            AppbarBackground = "#1E1E1E" ,
            DrawerBackground = "#1E1E1E" ,
            DrawerText = "#FFFFFF" ,
            DrawerIcon  = "#FFFFFF"
        };


        public static PaletteLight GetPaletteLight => new()
        {
            Primary          = "#00796B" , 
            Secondary        = "#009688" , 
            Tertiary         = "#4DB6AC" , 
            Info             = "#0288D1" , 
            Success          = "#26A69A" , 
            Warning          = "#FBC02D" , 
            Error            = "#D32F2F" , 
            Background       = "#FFFFFF" ,
            Surface    = "#F5F5F5" ,
            TextPrimary = "#212121" ,
            TextSecondary = "#757575" ,
            AppbarBackground = "#FFFFFF" ,
            DrawerBackground = "#FFFFFF" ,
            DrawerText = "#212121" ,
            DrawerIcon  = "#212121"
        };

        public static LayoutProperties GetLayoutProperties => new()
        {
            DefaultBorderRadius = "2px" ,
            DrawerMiniWidthLeft = "48px" ,
            DrawerMiniWidthRight = "48px" ,
            DrawerWidthLeft = "200px" ,
            DrawerWidthRight = "200px" ,
            AppbarHeight        = "56px"
        };
    }