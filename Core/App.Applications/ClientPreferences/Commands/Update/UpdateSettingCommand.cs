using MediatR;

namespace App.Applications.ClientPreferences.Commands.Update;

public class UpdateSettingCommand : IRequest
{
    public int Id { get; set; }
    public bool IsDarkMode { get; set; }
    public bool IsRtl { get; set; } = true;
    public string LanguageCode { get; set; } = "fa-Ir";

    public static UpdateSettingCommand CreateDefault()
    {
        return new UpdateSettingCommand
        {
            LanguageCode = "fa-IR" ,
            IsDarkMode   = true ,
            IsRtl        = true ,
        };
    }
}