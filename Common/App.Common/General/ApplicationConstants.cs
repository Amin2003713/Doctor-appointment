#region

using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;


#endregion

namespace App.Common.General;

public static class ApplicationConstants
{
    public static class Local
    {
        public const  string RefreshToken         = "refreshToken";
        public const  string UserImageUrl         = "userImageURL";
        public const  string CompanyIdentifier    = nameof(CompanyIdentifier);
        public const  string UserInfo             = nameof(UserInfo);
        public const  string AuthenticationSchema = nameof(UserInfo) + nameof(RefreshToken) + nameof(AuthenticationSchema);
        public const string ApiCacheKey          = $"ApiCache_{AuthenticationSchema}:";
    }

    public static class Server
    {
        public readonly static string ServerUrl = "localhost:7162";

        public readonly static string BaseUrl = $"https://{ServerUrl}";

        public readonly static RefitSettings RefitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy   = JsonNamingPolicy.CamelCase ,  
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull ,
            })
        };
    }


    public static class Headers
    {
        public const string UserName   = "X-User-Name";
        public const string Token      = "Bearer";
        public const string Hardware   = $"--{nameof(Hardware)}--";
        public const string DeviceType = "X-Device-Type";
    }




    public static class HttpStatusMessages
    {
        public const string AcceptedTitle            = "عملیات موفق";
        public const string InternalServerErrorTitle = "خطا";
        public const string WarningTitle             = "اخطار";
        public const string NotFoundTitle            = "ناموجود";
        public const string UnauthorizedTitle        = "دسترسی ممنوع";


        public const string SuccessMessage      = "عملیات با موفقیت انجام شد.";
        public const string ErrorMessage        = "خطایی رخ داده است.";
        public const string NotFoundMessage     = "آیتم مورد نظر یافت نشد.";
        public const string UnauthorizedMessage = "شما مجوز دسترسی را ندارید.";
    }
}