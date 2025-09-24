using Microsoft.Extensions.DependencyInjection;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Application.Services.Drivers;
using RouteScheduler.Core.Application.Services.Routes;
using RouteScheduler.Core.Application.Services.Schedules;

namespace RouteScheduler.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IScheduleService, ScheduleService>();

            return services;
        }
    }
}
