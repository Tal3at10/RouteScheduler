using RouteScheduler.Core.Domain.Contracts;

namespace RouteScheduler.APIs.Extensions
{
    public static class IntializerExtension
    {
        public static async Task<WebApplication> InitializeContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope(); // Create a scope to get scoped services like DbContext
            var services = scope.ServiceProvider; // Get the service provider from the scope

            var contextIntializer = services.GetRequiredService<IContextIntializer>();
            var loggerfactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await contextIntializer.InitializeAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger("app");
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }
            return app;
        }
    }
}
