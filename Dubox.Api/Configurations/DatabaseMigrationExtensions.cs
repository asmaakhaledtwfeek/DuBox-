using Dubox.Infrastructure.ApplicationContext;
using Dubox.Infrastructure.Services;
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

        /// <summary>
        /// Seeds schedule activities from the Excel baseline schedule file
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <param name="excelFilePath">Path to the Excel file (relative to the application directory or absolute)</param>
        /// <param name="projectId">Optional project ID to link activities to</param>
        /// <returns>The web application instance for chaining</returns>
        public static async Task<WebApplication> SeedScheduleActivitiesFromExcelAsync(
            this WebApplication app,
            string excelFilePath = "KJ-158 - Revised Baseline Schedule.xlsx",
            Guid? projectId = null)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                // Check if activities already exist
                var existingCount = await context.ScheduleActivities.CountAsync();
                if (existingCount > 0)
                {
                    logger.LogInformation("Schedule activities already seeded ({Count} activities exist). Skipping Excel import.", existingCount);
                    return app;
                }

                // Resolve file path - try multiple locations
                string? fullPath = null;
                
                if (Path.IsPathRooted(excelFilePath))
                {
                    // Absolute path provided
                    fullPath = excelFilePath;
                }
                else
                {
                    // Try multiple locations for relative path
                    var possiblePaths = new[]
                    {
                        Path.Combine(Directory.GetCurrentDirectory(), excelFilePath),
                        Path.Combine(AppContext.BaseDirectory, excelFilePath),
                        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", excelFilePath), // Go up from bin/Debug/net10.0
                        Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", excelFilePath)
                    };

                    foreach (var path in possiblePaths)
                    {
                        var normalizedPath = Path.GetFullPath(path);
                        if (File.Exists(normalizedPath))
                        {
                            fullPath = normalizedPath;
                            logger.LogInformation("Found Excel file at: {Path}", fullPath);
                            break;
                        }
                    }
                }

                if (fullPath == null || !File.Exists(fullPath))
                {
                    logger.LogWarning("Excel baseline schedule file not found. Tried locations:");
                    logger.LogWarning("  - {Path1}", Path.Combine(Directory.GetCurrentDirectory(), excelFilePath));
                    logger.LogWarning("  - {Path2}", Path.Combine(AppContext.BaseDirectory, excelFilePath));
                    logger.LogWarning("  - {Path3}", Path.Combine(AppContext.BaseDirectory, "..", "..", "..", excelFilePath));
                    logger.LogWarning("Skipping schedule activity seeding.");
                    logger.LogWarning("To fix: Place the Excel file at any of the above locations, or provide an absolute path.");
                    return app;
                }

                logger.LogInformation("Starting schedule activity seeding from Excel: {Path}", fullPath);

                // Use the service that handles indentation-based hierarchy
                var importService = new ScheduleActivityExcelImportService(context,
                    services.GetRequiredService<ILogger<ScheduleActivityExcelImportService>>());

                var result = await importService.ImportFromExcelAsync(fullPath, projectId);

                if (result.IsSuccess)
                {
                    logger.LogInformation(
                        "Schedule activity seeding completed successfully. Total: {Total}, Imported: {Imported}, Skipped: {Skipped}",
                        result.TotalProcessed, result.SuccessfullyImported, result.Skipped);

                    foreach (var warning in result.Warnings)
                    {
                        logger.LogWarning("Seeding warning: {Warning}", warning);
                    }
                }
                else
                {
                    logger.LogError("Schedule activity seeding failed with {Count} errors:", result.Errors.Count);
                    foreach (var error in result.Errors)
                    {
                        logger.LogError("  - {Error}", error);
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An unexpected error occurred during schedule activity seeding. Error: {ErrorMessage}", ex.Message);
                // Don't throw - allow the application to continue even if seeding fails
            }

            return app;
        }
    }
}















