using Microsoft.Extensions.Logging;

namespace Dubox.Api.Configurations;

/// <summary>
/// Helper class for diagnosing and configuring schedule activity seeding
/// </summary>
public static class ScheduleActivitySeedingHelper
{
    /// <summary>
    /// Logs all possible file locations that will be checked
    /// </summary>
    public static void LogPossibleFilePaths(ILogger logger, string relativeFilePath)
    {
        logger.LogInformation("=== Schedule Activity Seeding - File Path Diagnostic ===");
        logger.LogInformation("Looking for file: {FileName}", relativeFilePath);
        logger.LogInformation("");
        logger.LogInformation("Directory.GetCurrentDirectory(): {Path}", Directory.GetCurrentDirectory());
        logger.LogInformation("AppContext.BaseDirectory: {Path}", AppContext.BaseDirectory);
        logger.LogInformation("");
        logger.LogInformation("Will check these locations in order:");
        
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath),
            Path.Combine(AppContext.BaseDirectory, relativeFilePath),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", relativeFilePath),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", relativeFilePath)
        };

        for (int i = 0; i < possiblePaths.Length; i++)
        {
            var normalizedPath = Path.GetFullPath(possiblePaths[i]);
            var exists = File.Exists(normalizedPath);
            logger.LogInformation("  {Index}. {Status} {Path}", 
                i + 1, 
                exists ? "✓ FOUND   " : "✗ Not found", 
                normalizedPath);
        }
        
        logger.LogInformation("=== End Diagnostic ===");
    }

    /// <summary>
    /// Gets the recommended absolute path for the Excel file
    /// </summary>
    public static string GetRecommendedPath()
    {
        // Assumes the standard project structure
        var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
        return Path.GetFullPath(Path.Combine(projectRoot, "Documentation", "KJ-158 - Revised Baseline Schedule.xlsx"));
    }

    /// <summary>
    /// Finds the Excel file and returns its absolute path, or null if not found
    /// </summary>
    public static string? FindExcelFile(string relativeFilePath)
    {
        if (Path.IsPathRooted(relativeFilePath))
        {
            return File.Exists(relativeFilePath) ? relativeFilePath : null;
        }

        var possiblePaths = new[]
        {
            // First check workspace root (where the file is located)
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", relativeFilePath),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", relativeFilePath),
            // Then check current/base directories
            Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath),
            Path.Combine(AppContext.BaseDirectory, relativeFilePath)
        };

        foreach (var path in possiblePaths)
        {
            var normalizedPath = Path.GetFullPath(path);
            if (File.Exists(normalizedPath))
            {
                return normalizedPath;
            }
        }

        return null;
    }
}
