using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RouteScheduler.Infrastructure.Persistence.Data;

namespace RouteScheduler.Infrastructure.Persistence
{
    public static class DependencyInjection // static cuz it will contain extension methods only
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Context"));
            });

            return services;
        }
    }
}
