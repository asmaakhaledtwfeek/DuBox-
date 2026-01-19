using Dubox.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Api.Configurations
{
    public static class DatabaseMigrationExtensions
    {
        /// <summary>
        /// Automatically applies pending database migrations on application startup
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <param name="logger">Optional logger for tracking migration progress</param>
        /// <returns>The web application instance for chaining</returns>
        public static async Task<WebApplication> ApplyDatabaseMigrationsAsync(
            this WebApplication app, 
            ILogger? logger = null)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var log = logger ?? services.GetRequiredService<ILogger<Program>>();
                
                log.LogInformation("Checking for pending database migrations...");
                
                // Get pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                var pendingMigrationsList = pendingMigrations.ToList();
                
                if (pendingMigrationsList.Any())
                {
                    log.LogInformation($"Found {pendingMigrationsList.Count} pending migration(s). Applying migrations...");
                    
                    foreach (var migration in pendingMigrationsList)
                    {
                        log.LogInformation($"  - {migration}");
                    }
                    
                    // Apply all pending migrations
                    await context.Database.MigrateAsync();
                    
                    log.LogInformation("Database migrations applied successfully.");
                }
                else
                {
                    log.LogInformation("Database is up to date. No pending migrations.");
                }
                
                // Verify database can be connected to
                var canConnect = await context.Database.CanConnectAsync();
                if (canConnect)
                {
                    log.LogInformation("Database connection verified successfully.");
                }
                else
                {
                    log.LogWarning("Database connection could not be verified.");
                }
            }
            catch (Exception ex)
            {
                var log = logger ?? services.GetRequiredService<ILogger<Program>>();
                log.LogError(ex, "An error occurred while applying database migrations. Error: {ErrorMessage}", ex.Message);
                
                // Optionally, you can choose to throw or continue
                // For production, you might want to throw to prevent app startup with DB issues
                throw;
            }
            
            return app;
        }
        
        /// <summary>
        /// Ensures the database is created and applies all pending migrations
        /// Use this for development environments
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <returns>The web application instance for chaining</returns>
        public static async Task<WebApplication> EnsureDatabaseCreatedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Ensuring database is created...");
                
                // This will create the database if it doesn't exist and apply all migrations
                await context.Database.MigrateAsync();
                
                logger.LogInformation("Database is ready.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while ensuring database creation. Error: {ErrorMessage}", ex.Message);
                throw;
            }
            
            return app;
        }
    }
}




