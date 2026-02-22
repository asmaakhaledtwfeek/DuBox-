using Dubox.Application.Abstractions;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Dubox.Application.Infrastructure;

/// <summary>
/// Base class for configuration processors with common functionality
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TDto">The DTO type</typeparam>
public abstract class ConfigurationProcessorBase<TEntity, TDto> : IConfigurationProcessor<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger Logger;

    protected ConfigurationProcessorBase(IUnitOfWork unitOfWork, ILogger logger)
    {
        UnitOfWork = unitOfWork;
        Logger = logger;
    }

    /// <summary>
    /// Validates if entities can be deleted and deletes those that are not in use
    /// </summary>
    public abstract Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId,
        List<TDto> requestDtos,
        IEnumerable<Box> projectBoxes,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates existing entities or adds new ones
    /// </summary>
    public abstract Task<List<int>> UpdateOrAddAsync(
        Guid projectId,
        List<TDto> requestDtos,
        CancellationToken cancellationToken);

    /// <summary>
    /// Helper method to log errors
    /// </summary>
    protected void LogError(string message, Exception? exception = null)
    {
        if (exception != null)
        {
            Logger.LogError(exception, message);
        }
        else
        {
            Logger.LogError(message);
        }
    }
}
