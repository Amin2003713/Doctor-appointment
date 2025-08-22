#region

    using UI.Components.Services;
    using MediatR;

#endregion

    namespace App.Services;

    public static class DependencyInjections
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddComponentServices();
        }
    }