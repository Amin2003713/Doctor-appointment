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
            public const string RefreshToken         = "refreshToken";
            public const string UserImageUrl         = "userImageURL";
            public const string CompanyIdentifier    = nameof(CompanyIdentifier);
            public const string UserInfo             = nameof(UserInfo);
            public const string AuthenticationSchema = nameof(UserInfo) + nameof(RefreshToken) + nameof(AuthenticationSchema);
            public const string ApiCacheKey          = $"ApiCache_{AuthenticationSchema}:";
        }

        public static class Server
        {
            public readonly static string ServerUrl = "localhost:7162";

            public readonly static string BaseUrl = $"https://{ServerUrl}";

            public readonly static RefitSettings RefitSettings = new()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    PropertyNamingPolicy   = JsonNamingPolicy.CamelCase ,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
    
    public static class ApiRoutes
    {
        public static class Clinic
        {
            private const string Base = "api/clinic";
            public const string GetSettings    = $"{Base}/settings";
            public const string UpdateSettings = $"{Base}/settings";
        }
    
        public static class Doctor
        {
            private const string Base = "api/doctor";
            public const string GetProfile    = $"{Base}/profile";
            public const string UpsertProfile = $"{Base}/profile";
        }
    
        public static class Services
        {
            private const string Base = "api/services";
            public const string List   = $"{Base}";
            public const string Get    = $"{Base}/{{id:guid}}";
            public const string Create = $"{Base}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
    
        public static class Appointments
        {
            private const string Base = "api/appointments";
            public const string List       = $"{Base}";
            public const string GetById    = $"{Base}/{{id:guid}}";
            public const string Create     = $"{Base}";
            public const string Cancel     = $"{Base}/{{id:guid}}/cancel";
            public const string Complete   = $"{Base}/{{id:guid}}/complete";
            public const string Reschedule = $"{Base}/{{id:guid}}/reschedule";
        }
    
        public static class Schedule
        {
            private const string Base = "api/schedule";
            public const string Get    = $"{Base}";
            public const string Update = $"{Base}";
            public const string Slots  = $"{Base}/slots";
        }

        public static class User
        {
            public const string Base = "/api/User";

            // Auth
            public const string Register          = $"{Base}/register";
            public const string RegisterPatient   = $"{Base}/register/patient";
            public const string RegisterSecretary = $"{Base}/register/secretary";
            public const string ChangeRole        = $"{Base}/change-role";
            public const string Login             = $"{Base}/login";
            public const string ForgotPassword    = $"{Base}/forgot-password";
            public const string Me                = $"{Base}/me";

            // Users
            public const string Users            = $"{Base}/users";
            public const string UsersSecretaries = $"{Base}/users/secretaries";

            // Template routes (must be const for attributes)
            public const string UserByIdTemplate = $"{Base}/user/{{id}}";
            public const string ToggleTemplate   = $"{Base}/toggle";

        }
    }