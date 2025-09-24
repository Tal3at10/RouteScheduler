using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Infrastructure.Persistence.Data;
using RouteScheduler.Infrastructure.Persistence.Data.Mongo;
using RouteScheduler.Infrastructure.Persistence.unitOfWork;

namespace RouteScheduler.Infrastructure.Persistence
{
    public static class DependencyInjection // static cuz it will contain extension methods only
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = (configuration["DatabaseProvider"] ?? "sqlserver").ToLowerInvariant();

            if (provider == "mongodb")
            {
                var mongoSettings = new MongoSettings
                {
                    ConnectionUri = configuration.GetConnectionString("MongoDb") ?? configuration["Mongo:ConnectionUri"] ?? string.Empty,
                    DatabaseName = configuration["Mongo:DatabaseName"] ?? "routescheduler"
                };
                services.AddSingleton(mongoSettings);
                services.AddSingleton<MongoContext>();
                services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
            }
            else
            {
                services.AddDbContext<Context>(optionsBuilder =>
                {
                    switch (provider)
                    {
                        case "sqlite":
                            optionsBuilder.UseSqlite(configuration.GetConnectionString("Sqlite"));
                            break;
                        case "postgres":
                        case "postgresql":
                            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgres"));
                            break;
                        default:
                            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServer") ?? configuration.GetConnectionString("Context"));
                            break;
                    }
                });

                services.AddScoped<IContextIntializer, ContextIntializer>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            }
            
            return services;
        }
    }
}
