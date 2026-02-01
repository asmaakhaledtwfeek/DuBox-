using Dubox.Application.Features.Cost.Commands;
using MediatR;

namespace Dubox.Api.Services;

/// <summary>
/// Background service that automatically seeds cost data on application startup
/// </summary>
public class DataSeederHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSeederHostedService> _logger;
    private readonly IWebHostEnvironment _environment;

    public DataSeederHostedService(
        IServiceProvider serviceProvider,
        ILogger<DataSeederHostedService> logger,
        IWebHostEnvironment environment)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _environment = environment;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🌱 DataSeeder: Starting automatic data seeding in background...");
        _logger.LogInformation("💡 DataSeeder: Application will start immediately while seeding runs in background");

        // Run seeding in background - don't block startup
        _ = Task.Run(async () => await ExecuteSeedingAsync(cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }

    private async Task ExecuteSeedingAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("📊 DataSeeder: Background seeding task started...");
            _logger.LogInformation("💡 DataSeeder: Using batch processing (5 records per batch) for optimal performance");
            _logger.LogInformation("🚀 DataSeeder: Importing both files in parallel for faster performance");

            // Check if data directory exists
            var dataPath = Path.Combine(_environment.WebRootPath, "data");
            if (!Directory.Exists(dataPath))
            {
                _logger.LogWarning("⚠️ DataSeeder: Data directory not found at {Path}. Skipping seed.", dataPath);
                return;
            }

            var importTasks = new List<Task>();
            var overallStartTime = DateTime.UtcNow;

            // Import Cost Codes Master file (in parallel)
            var costCodeFile = Path.Combine(dataPath, "Cost Codes Master - Amana.xlsx");
            if (File.Exists(costCodeFile))
            {
                _logger.LogInformation("📊 DataSeeder: Found Cost Codes Master file. Starting import...");

                var costCodeTask = Task.Run(async () =>
                {
                    // Create a separate scope for this import to get its own DbContext instance
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    try
                    {
                        var importCommand = new ImportCostCodesCommand
                        {
                            FilePath = costCodeFile,
                            ClearExisting = false
                        };

                        var startTime = DateTime.UtcNow;
                        var result = await mediator.Send(importCommand, cancellationToken);
                        var duration = (DateTime.UtcNow - startTime).TotalSeconds;

                        if (result.IsSuccess)
                        {
                            _logger.LogInformation(
                                "✅ DataSeeder: Cost codes seeded in {Duration:F2}s! Total: {Total}, Success: {Success}, Skipped: {Skipped}, Errors: {Errors}",
                                duration,
                                result.Data?.TotalRecords ?? 0,
                                result.Data?.SuccessCount ?? 0,
                                result.Data?.SkippedCount ?? 0,
                                result.Data?.ErrorCount ?? 0);
                            
                            if (result.Data?.Errors?.Count > 0)
                            {
                                _logger.LogWarning("⚠️ DataSeeder: {ErrorCount} errors occurred during cost code import", result.Data.Errors.Count);
                            }
                        }
                        else
                        {
                            _logger.LogError("❌ DataSeeder: Failed to seed cost codes.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ DataSeeder: Error importing Cost Codes Master file");
                    }
                }, cancellationToken);

                importTasks.Add(costCodeTask);
            }
            else
            {
                _logger.LogWarning("⚠️ DataSeeder: Cost Codes Master file not found.");
            }

            // Import HR Costs file (in parallel)
            var hrCostFile = Path.Combine(dataPath, "EportHRC 1.xlsx");
            if (File.Exists(hrCostFile))
            {
                _logger.LogInformation("👥 DataSeeder: Found HRC file (EportHRC 1.xlsx). Starting import...");

                var hrCostTask = Task.Run(async () =>
                {
                    // Create a separate scope for this import to get its own DbContext instance
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    try
                    {
                        var importHRCommand = new ImportHRCostsCommand
                        {
                            FilePath = hrCostFile,
                            ClearExisting = false
                        };

                        var startTime = DateTime.UtcNow;
                        var hrResult = await mediator.Send(importHRCommand, cancellationToken);
                        var duration = (DateTime.UtcNow - startTime).TotalSeconds;

                        if (hrResult.IsSuccess)
                        {
                            _logger.LogInformation(
                                "✅ DataSeeder: HRC codes seeded in {Duration:F2}s! Total: {Total}, Success: {Success}, Skipped: {Skipped}, Errors: {Errors}",
                                duration,
                                hrResult.Data?.TotalRecords ?? 0,
                                hrResult.Data?.SuccessCount ?? 0,
                                hrResult.Data?.SkippedCount ?? 0,
                                hrResult.Data?.ErrorCount ?? 0);
                            
                            if (hrResult.Data?.Errors?.Count > 0)
                            {
                                _logger.LogWarning("⚠️ DataSeeder: {ErrorCount} errors occurred during HRC import", hrResult.Data.Errors.Count);
                            }
                        }
                        else
                        {
                            _logger.LogError("❌ DataSeeder: Failed to seed HRC codes.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ DataSeeder: Error importing HRC file");
                    }
                }, cancellationToken);

                importTasks.Add(hrCostTask);
            }
            else
            {
                _logger.LogWarning("⚠️ DataSeeder: HRC file (EportHRC 1.xlsx) not found.");
            }

            // Wait for all imports to complete
            if (importTasks.Count > 0)
            {
                await Task.WhenAll(importTasks);
                var totalDuration = (DateTime.UtcNow - overallStartTime).TotalSeconds;
                _logger.LogInformation("⚡ DataSeeder: All imports completed in {TotalDuration:F2}s (parallel execution)", totalDuration);
            }
            else
            {
                _logger.LogWarning("⚠️ DataSeeder: No files found to import.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ DataSeeder: An error occurred during background data seeding");
        }

        _logger.LogInformation("🏁 DataSeeder: Background data seeding completed.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DataSeeder: Stopping...");
        return Task.CompletedTask;
    }
}

