using RouteScheduler.APIs.Middleware;
using RouteScheduler.Core.Application;
using RouteScheduler.Core.Application.Mapping;
using RouteScheduler.APIs.Extensions;
using RouteScheduler.Infrastructure.Persistence;

namespace RouteScheduler.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            // Swagger
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

            // Application Services
            builder.Services.AddApplicationServices();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Persistence Services (EF Core / MongoDB)
            builder.Services.AddPersistenceServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region Configure Middleware

            // Swagger middleware for all environments (can restrict to Development if needed)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Route Scheduler API v1");
                c.RoutePrefix = "swagger"; // Swagger UI available at /swagger/index.html
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthorization();
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
