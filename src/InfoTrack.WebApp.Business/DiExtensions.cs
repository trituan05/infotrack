using InfoTrack.WebApp.Business.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.WebApp.Business
{
    public static class DiExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IDashboardManager, DashboardManager>();
            return services;
        }
    }
}
