#region

    using UI.Components.Services;

#endregion

    namespace UI.Web.Services;

    public static class DependencyInjections
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddComponentServices();
        }
    }