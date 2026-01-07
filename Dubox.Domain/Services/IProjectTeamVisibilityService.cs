namespace Dubox.Domain.Services;

public interface IProjectTeamVisibilityService
{
   
    Task<bool> CanCreateProjectOrTeamAsync(CancellationToken cancellationToken = default);

    Task<List<Guid>?> GetAccessibleProjectIdsAsync(CancellationToken cancellationToken = default);

    Task<List<Guid>?> GetAccessibleTeamIdsAsync(CancellationToken cancellationToken = default);

    Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<bool> CanAccessTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<bool> CanModifyDataAsync(CancellationToken cancellationToken = default);

    Task<bool> CanEditProjectAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<bool> IsProjectArchivedAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<bool> IsProjectOnHoldAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<bool> IsProjectClosedAsync(Guid projectId, CancellationToken cancellationToken = default);
}

