namespace App.Common.General;

public static class ApiRoutes
{
    public static class Clinic
    {
        private const string Base           = "/api/clinic";
        public const  string GetSettings    = $"{Base}/settings";
        public const  string UpdateSettings = $"{Base}/settings";
    }

    public static class Doctor
    {
        private const string Base          = "/api/doctor";
        public const  string GetProfile    = $"{Base}/profile";
        public const  string UpsertProfile = $"{Base}/profile";
    }

    public static class Services
    {
        private const string Base   = "/api/services";
        public const  string List   = $"{Base}";
        public const  string Get    = $"{Base}/{{id}}";
        public const  string Create = $"{Base}";
        public const  string Update = $"{Base}/{{id}}";
        public const  string Delete = $"{Base}/{{id}}";
    }

    public static class Appointments
    {
        private const string Base       = "/api/appointments";
        public const  string List       = $"{Base}";
        public const  string GetById    = $"{Base}/{{id}}";
        public const  string Create     = $"{Base}";
        public const  string Cancel     = $"{Base}/{{id}}/cancel";
        public const  string Complete   = $"{Base}/{{id}}/complete";
        public const  string Reschedule = $"{Base}/{{id}}/reschedule";
    }

    public static class Schedule
    {
        private const string Base        = "/api/schedule";
        public const  string Get         = $"{Base}";
        public const  string Update      = $"{Base}";
        public const  string Slots       = $"{Base}/slots";
        public const  string SlotSummery = $"{Base}/slots/summary";
    }

    public static class User
    {
        public const string Base = "/api/User";

        
        public const string Register          = $"{Base}/register";
        public const string RegisterPatient   = $"{Base}/register/patient";
        public const string RegisterSecretary = $"{Base}/register/secretary";
        public const string ChangeRole        = $"{Base}/change-role";
        public const string Login             = $"{Base}/login";
        public const string ForgotPassword    = $"{Base}/forgot-password";
        public const string Me                = $"{Base}/me";

        
        public const string Users            = $"{Base}/users";
        public const string UsersSecretaries = $"{Base}/users/secretaries";

        
        public const string UserByIdTemplate = $"{Base}/user/{{id}}";
        public const string ToggleTemplate   = $"{Base}/toggle";
    }
}