using InfoTrack.Services.Google.Services;
using Microsoft.Extensions.DependencyInjection;
namespace InfoTrack.Services.Google
{
    public static class DiExtensions
    {
        public static IServiceCollection AddGoogleSearch(this IServiceCollection services)
        {
            services.AddHttpClient<IGoogleSearchService, GoogleSearchService>()
                .ConfigureHttpClient((sp, client) =>
                {
                    client.BaseAddress = new Uri("https://www.google.co.uk/", UriKind.Absolute);
                });

            return services;
        }
    }
}
