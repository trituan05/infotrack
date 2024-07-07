using InfoTrack.DataAccess.DbContexts;
using InfoTrack.DataAccess.Repository;
using InfoTrack.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.DataAccess
{
    public static class DiExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));
            services.AddScoped<IRankingRepository, RankingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            return services;
        }
    }
}
