using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.EntityFramework
{
    public static class Registration
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration, ServiceLifetime? serviceLifetime = ServiceLifetime.Scoped)
        {
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString),
                serviceLifetime ?? ServiceLifetime.Scoped);

            if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IUnitOfWork, UnitOfWork>();
            }
            else
            {
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            }
        }
    }
}
