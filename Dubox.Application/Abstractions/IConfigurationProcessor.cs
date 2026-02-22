using Dubox.Domain.Entities;

namespace Dubox.Application.Abstractions;

/// <summary>
/// Generic interface for processing project configuration entities
/// </summary>
/// <typeparam name="TEntity">The entity type (ProjectBuilding, ProjectLevel, etc.)</typeparam>
/// <typeparam name="TDto">The DTO type for the entity</typeparam>
public interface IConfigurationProcessor<TEntity, TDto> 
    where TEntity : class
    where TDto : class
{
    /// <summary>
    /// Validates if entities can be deleted (checks for usage in boxes)
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="requestDtos">The DTOs from the request</param>
    /// <param name="projectBoxes">All boxes in the project for usage checking</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of error messages for entities that cannot be deleted</returns>
    Task<List<string>> ValidateAndDeleteAsync(
        Guid projectId, 
        List<TDto> requestDtos, 
        IEnumerable<Box> projectBoxes, 
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates existing entities or adds new ones
    /// </summary>
    /// <param name="projectId">The project ID</param>
    /// <param name="requestDtos">The DTOs from the request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of newly created entity IDs (if applicable)</returns>
    Task<List<int>> UpdateOrAddAsync(
        Guid projectId, 
        List<TDto> requestDtos, 
        CancellationToken cancellationToken);
}
