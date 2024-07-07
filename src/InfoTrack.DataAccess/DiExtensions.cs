using InfoTrack.DataAccess.DbContexts;
using InfoTrack.DataAccess.Repository;
using InfoTrack.DataAccess.Sql;
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
                options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("InfoTrack.DataAccess")));
            services.AddScoped<IRankingRepository, RankingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddHostedService<Migrator>();
            return services;
        }
    }
}
