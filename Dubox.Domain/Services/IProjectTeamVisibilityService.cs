namespace Dubox.Domain.Services;

/// <summary>
/// Service to handle project and team visibility rules based on user roles
/// </summary>
public interface IProjectTeamVisibilityService
{
    /// <summary>
    /// Check if the current user can create projects or teams
    /// Only SystemAdmin and ProjectManager can create
    /// </summary>
    Task<bool> CanCreateProjectOrTeamAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the list of project IDs that the current user can access
    /// Returns null if user can access all projects (SystemAdmin)
    /// Returns list of project IDs for restricted access
    /// </summary>
    Task<List<Guid>?> GetAccessibleProjectIdsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the list of team IDs that the current user can access
    /// Returns null if user can access all teams (SystemAdmin)
    /// Returns list of team IDs for restricted access
    /// </summary>
    Task<List<Guid>?> GetAccessibleTeamIdsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if the current user can access a specific project
    /// </summary>
    Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if the current user can access a specific team
    /// </summary>
    Task<bool> CanAccessTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
}

