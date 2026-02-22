using Dubox.Domain.Entities;
using Dubox.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Seed data for Schedule Activities from Excel file
/// </summary>
public static class ScheduleActivitySeedData
{
    /// <summary>
    /// Seed schedule activities from an Excel file at design time (in migrations)
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder for migrations</param>
    /// <param name="excelFilePath">Path to the Excel file</param>
    public static void SeedScheduleActivitiesFromExcel(ModelBuilder modelBuilder, string excelFilePath)
    {
        // This is a placeholder for design-time seeding
        // Actual seeding should be done at runtime using the service
        // Because EPPlus requires full .NET runtime
    }

    /// <summary>
    /// Seed schedule activities at runtime using the import service
    /// Call this from a migration's Up() method or from application startup
    /// </summary>
    public static async Task SeedScheduleActivitiesAsync(
        ApplicationContext.ApplicationDbContext context,
        ILogger logger,
        string excelFilePath,
        Guid? projectId = null)
    {
        try
        {
            // Check if activities already exist
            var existingCount = await context.ScheduleActivities.CountAsync();
            if (existingCount > 0)
            {
                logger.LogInformation("Schedule activities already seeded. Skipping.");
                return;
            }

            if (!File.Exists(excelFilePath))
            {
                logger.LogWarning("Excel file not found at: {Path}. Skipping schedule activity seeding.", excelFilePath);
                return;
            }

            logger.LogInformation("Starting schedule activity seeding from Excel: {Path}", excelFilePath);

            var importService = new ScheduleActivityExcelImportService(context, 
                LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ScheduleActivityExcelImportService>());

            var result = await importService.ImportFromExcelAsync(excelFilePath, projectId);

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
            logger.LogError(ex, "Error during schedule activity seeding");
        }
    }
}
