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

    /// <summary>
    /// Check if the current user can modify data (create, update, delete)
    /// Viewer role has read-only access and cannot modify data
    /// </summary>
    Task<bool> CanModifyDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a project is archived
    /// Archived projects are read-only and cannot be modified
    /// </summary>
    Task<bool> IsProjectArchivedAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a project is on hold
    /// OnHold projects only allow status changes based on progress
    /// </summary>
    Task<bool> IsProjectOnHoldAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a project is closed
    /// Closed projects only allow status changes, no other actions are permitted
    /// </summary>
    Task<bool> IsProjectClosedAsync(Guid projectId, CancellationToken cancellationToken = default);
}

