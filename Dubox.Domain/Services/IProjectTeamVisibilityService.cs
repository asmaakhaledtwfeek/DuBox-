using Dubox.Domain.Enums;
using Dubox.Domain.Shared;

namespace Dubox.Domain.Services;

public interface IProjectTeamVisibilityService
{
    Task<List<Guid>?> GetAccessibleProjectIdsAsync(CancellationToken cancellationToken = default);
    Task<List<Guid>?> GetAccessibleTeamIdsAsync(CancellationToken cancellationToken = default);
    Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<bool> CanAccessTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<bool> CanPerformAsync(PermissionModuleEnum module, PermissionActionEnum action, CancellationToken cancellationToken = default);
    Task<Result<bool>> GetProjectStatusChecksAsync(Guid projectId, string actionName, CancellationToken cancellationToken = default);
    Task<Result<bool>> GetBoxStatusChecksAsync(Guid boxId, string actionName, CancellationToken cancellationToken = default);
    Task<Result<bool>> GetActivityStatusChecksAsync(Guid activityId, string actionName, CancellationToken cancellationToken = default);

}

