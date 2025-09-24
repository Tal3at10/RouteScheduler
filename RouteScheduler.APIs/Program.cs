using RouteScheduler.APIs.Middleware;
using RouteScheduler.Core.Application;
using RouteScheduler.Core.Application.Mapping;
using RouteScheduler.Infrastructure.Persistence;

namespace RouteScheduler.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "Route Scheduler API",
                    Version = "v1",
                    Description = "A comprehensive API for managing drivers, routes, and schedules"
                });
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddPersistenceServices(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Route Scheduler API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
