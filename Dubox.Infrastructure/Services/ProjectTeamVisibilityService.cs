using Dubox.Domain.Abstraction;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Services;

public class ProjectTeamVisibilityService : IProjectTeamVisibilityService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDbContext _context;
    private readonly IUserRoleService _userRoleService;
    IPermissionService _permissionService;
    // Role names as constants
    private  string SystemAdminRole = SystemRoleEnum.SystemAdmin.ToString();
    private  string ProjectManagerRole = SystemRoleEnum.ProjectManager.ToString();
    private  string ViewerRole = SystemRoleEnum.Viewer.ToString();
    private  string QCInspectorRole = SystemRoleEnum.QCInspector.ToString();

    public ProjectTeamVisibilityService(
        ICurrentUserService currentUserService,
        IDbContext context,
        IUserRoleService userRoleService ,
        IPermissionService permissionService)
    {
        _currentUserService = currentUserService;
        _context = context;
        _userRoleService = userRoleService;
        _permissionService = permissionService;

    }
    public async Task<List<Guid>?> GetAccessibleProjectIdsAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            return new List<Guid>();

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            return new List<Guid>();

        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);

        if (isSystemAdmin || isViewer)
        {
            return null; // null means access to ALL projects
        }

        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            var pmOwnProjects = await _context.Projects
                .Where(p => p.CreatedBy == userId || p.ProjectMangerId==userId)
                .Select(p => p.ProjectId)
                .ToListAsync(cancellationToken);

            var systemAdminProjects = await GetSystemAdminProjectsForUserAsync(userId, cancellationToken);
            var pmAccessibleProjects = pmOwnProjects.Union(systemAdminProjects).Distinct().ToList();

            return pmAccessibleProjects;
        }

        var isQCInspector = await _userRoleService.UserHasRoleAsync(userId, QCInspectorRole, cancellationToken);
        if (isQCInspector)
        { 
           var inspectorProjects = await GetInspectorProjectsForUserAsync(userId, cancellationToken);
            
            var ownProjects = await _context.Projects
                .Where(p => p.CreatedBy == userId)
                .Select(p => p.ProjectId)
                .ToListAsync(cancellationToken);

            var teamCreatorProjects = await GetAllTeamCreatorProjectsForUserAsync(userId, cancellationToken);

            var allQCInspectorProjects = ownProjects
                .Union(inspectorProjects)
                .Union(teamCreatorProjects)
                .Distinct()
                .ToList();

            return allQCInspectorProjects;
        }

        var userOwnProjects = await _context.Projects
            .Where(p => p.CreatedBy == userId)
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        var userTeamCreatorProjects = await GetAllTeamCreatorProjectsForUserAsync(userId, cancellationToken);

        var allAccessibleProjects = userOwnProjects.Union(userTeamCreatorProjects).Distinct().ToList();

        return allAccessibleProjects;
    }
    private async Task<List<Guid>> GetAllTeamCreatorProjectsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userTeams = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.Team)
            .ToListAsync(cancellationToken);

        if (!userTeams.Any())
            return new List<Guid>();

        var teamCreatorIds = userTeams
            .Where(t => t.CreatedBy.HasValue)
            .Select(t => t.CreatedBy!.Value)
            .Distinct()
            .ToList();

        if (!teamCreatorIds.Any())
            return new List<Guid>();

        var projectIds = await _context.Projects
            .Where(p => p.CreatedBy.HasValue && teamCreatorIds.Contains(p.CreatedBy!.Value))
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        return projectIds;
    }
    private async Task<List<Guid>> GetSystemAdminProjectsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userTeams = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.Team)
            .ToListAsync(cancellationToken);

        if (!userTeams.Any())
            return new List<Guid>();

        var systemAdminIds = new List<Guid>();
        foreach (var team in userTeams)
        {
            if (team.CreatedBy.HasValue)
            {
                var isSystemAdmin = await _userRoleService.UserHasRoleAsync(team.CreatedBy.Value, SystemAdminRole, cancellationToken);
                if (isSystemAdmin)
                    systemAdminIds.Add(team.CreatedBy.Value);
            }
        }

        if (!systemAdminIds.Any())
            return new List<Guid>();

        var projectIds = await _context.Projects
            .Where(p => p.CreatedBy.HasValue && systemAdminIds.Contains(p.CreatedBy.Value))
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        return projectIds;
    }
    
    private async Task<List<Guid>> GetInspectorProjectsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Find all projects where the user is assigned as an inspector in any checkpoints
        var projectIds = await _context.WIRCheckpoints
            .Where(wc => wc.InspectorId.HasValue && wc.InspectorId.Value == userId)
            .Select(wc => wc.Box.ProjectId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return projectIds;
    }
    public async Task<List<Guid>?> GetAccessibleTeamIdsAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            return new List<Guid>();

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            return new List<Guid>();

        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);

        if (isSystemAdmin || isViewer)
        {
            return null;
        }

        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            var createdTeams = await _context.Teams
                .Where(t => t.CreatedBy == userId)
                .Select(t => t.TeamId)
                .ToListAsync(cancellationToken);

            var memberTeams = await _context.TeamMembers
                .Where(tm => tm.UserId == userId && tm.IsActive)
                .Select(tm => tm.TeamId)
                .ToListAsync(cancellationToken);

            var allPMTeams = createdTeams.Union(memberTeams).Distinct().ToList();
            return allPMTeams;
        }

        var memberTeamIds = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.TeamId)
            .ToListAsync(cancellationToken);

        return memberTeamIds;
    }

    public async Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var accessibleProjectIds = await GetAccessibleProjectIdsAsync(cancellationToken);

        if (accessibleProjectIds == null)
            return true;

        return accessibleProjectIds.Contains(projectId);
    }

    public async Task<bool> CanAccessTeamAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        var accessibleTeamIds = await GetAccessibleTeamIdsAsync(cancellationToken);

        // null means access to all teams
        if (accessibleTeamIds == null)
        {
            return true;
        }

        return accessibleTeamIds.Contains(teamId);
    }


    public async Task<bool> CanPerformAsync( PermissionModuleEnum module, PermissionActionEnum action, CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
            return false;

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            return false;

        if (action != PermissionActionEnum.View)
        {
            var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);
            if (isViewer)
                return false;
        }

        return await _permissionService
            .UserHasPermissionAsync(userId, module, action, cancellationToken);
    }

    public async Task<Result<bool>> GetProjectStatusChecksAsync(Guid projectId, string actionName,CancellationToken cancellationToken = default)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectId == projectId, cancellationToken);

        if (project == null)
            return Result.Failure<bool>("Project not found");
        if(project.Status == ProjectStatusEnum.Archived )
            return Result.Failure<bool>(
                 $"Cannot {actionName} in an archived project. Archived projects are read-only.");
        if (project.Status == ProjectStatusEnum.OnHold)
            return Result.Failure<bool>(
                 $"Cannot {actionName} in an on-hold project. On-hold projects only allow project status changes.");
        if (project.Status == ProjectStatusEnum.Closed )
            return Result.Failure<bool>(
                 $"Cannot {actionName} in a closed project. Closed projects only allow project status changes.");
        return Result.Success(true);
    }
    public async Task<Result<bool>> GetBoxStatusChecksAsync(Guid boxId, string actionName, CancellationToken cancellationToken = default)
    {
        var box = await _context.Boxes
            .FirstOrDefaultAsync(b => b.BoxId == boxId, cancellationToken);

        if (box == null)
            return Result.Failure<bool>("Box not found");
        if (box.Status == BoxStatusEnum.Dispatched)
            return Result.Failure<bool>(
                 $"Cannot {actionName}. The box is dispatched and no actions are allowed. Only viewing is permitted.");
        if (box.Status == BoxStatusEnum.OnHold)
            return Result.Failure<bool>(
                 $"Cannot {actionName}. The box is on hold and no actions are allowed. Only viewing is permitted.");

        return Result.Success(true);
    }
    public async Task<Result<bool>> GetActivityStatusChecksAsync(Guid activityId, string actionName, CancellationToken cancellationToken = default)
    {
        var activity = await _context.BoxActivities
            .FirstOrDefaultAsync(b => b.BoxActivityId == activityId, cancellationToken);

        if (activity == null)
            return Result.Failure<bool>("Box Activity not found");
        if (activity.Status == BoxStatusEnum.Completed || activity.Status == BoxStatusEnum.Delayed)
            return Result.Failure<bool>(
                 $"Cannot {actionName}. Activities in 'Completed' or 'Delayed' status cannot be modified.");
        if (activity.Status == BoxStatusEnum.OnHold)
            return Result.Failure<bool>(
                 $"Cannot {actionName}. Activities in 'OnHold' status cannot be modified. Please change the activity status first.");

        return Result.Success(true);
    }
   
}

