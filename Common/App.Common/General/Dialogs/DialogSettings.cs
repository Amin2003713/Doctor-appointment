using MudBlazor;

namespace App.Common.General.Dialogs;

public static class DialogSettings
{
    public static DialogOptions BlurBlackGround(
        bool closeButton = true ,
        DialogPosition? position = DialogPosition.Center ,
        MaxWidth? maxWidth = MaxWidth.Small)
        => new DialogOptions
    {
        CloseOnEscapeKey = false ,
        Position         = position ,
        CloseButton      = closeButton ,
        BackgroundClass  = "back-ground-blur" ,
        MaxWidth         = maxWidth,
    };
}