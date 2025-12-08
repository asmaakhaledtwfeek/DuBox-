using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Services;

public class ProjectTeamVisibilityService : IProjectTeamVisibilityService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDbContext _context;
    private readonly IUserRoleService _userRoleService;

    // Role names as constants
    private const string SystemAdminRole = "SystemAdmin";
    private const string ProjectManagerRole = "ProjectManager";

    public ProjectTeamVisibilityService(
        ICurrentUserService currentUserService,
        IDbContext context,
        IUserRoleService userRoleService)
    {
        _currentUserService = currentUserService;
        _context = context;
        _userRoleService = userRoleService;
    }

    public async Task<bool> CanCreateProjectOrTeamAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return false;
        }

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return false;
        }

        // Only SystemAdmin and ProjectManager can create projects/teams
        return await _userRoleService.UserHasAnyRoleAsync(
            userId, 
            new[] { SystemAdminRole, ProjectManagerRole }, 
            cancellationToken);
    }

    public async Task<List<Guid>?> GetAccessibleProjectIdsAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return new List<Guid>(); // No access
        }

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return new List<Guid>(); // No access
        }

        // Check if user is SystemAdmin
        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        if (isSystemAdmin)
        {
            return null; // null means access to ALL projects
        }

        // Check if user is ProjectManager
        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            // Return only projects created by this user
            var projectIds = await _context.Projects
                .Where(p => p.CreatedBy == userId.ToString())
                .Select(p => p.ProjectId)
                .ToListAsync(cancellationToken);
            
            return projectIds;
        }

        // For all other roles: find projects created by the PM who created the user's team
        var userTeam = await _context.TeamMembers
            .Where(tm => tm.UserId == userId)
            .Select(tm => tm.Team)
            .FirstOrDefaultAsync(cancellationToken);

        if (userTeam == null || !userTeam.CreatedBy.HasValue)
        {
            return new List<Guid>(); // User not in any team or team has no creator
        }

        // Get projects created by the PM who created the team
        var accessibleProjectIds = await _context.Projects
            .Where(p => p.CreatedBy == userTeam.CreatedBy.Value.ToString())
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        return accessibleProjectIds;
    }

    public async Task<List<Guid>?> GetAccessibleTeamIdsAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return new List<Guid>(); // No access
        }

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return new List<Guid>(); // No access
        }

        // Check if user is SystemAdmin
        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        if (isSystemAdmin)
        {
            return null; // null means access to ALL teams
        }

        // Check if user is ProjectManager
        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            // Return only teams created by this user
            var teamIds = await _context.Teams
                .Where(t => t.CreatedBy == userId)
                .Select(t => t.TeamId)
                .ToListAsync(cancellationToken);
            
            return teamIds;
        }

        // For all other roles: find teams created by the PM who created the user's team
        var userTeam = await _context.TeamMembers
            .Where(tm => tm.UserId == userId)
            .Select(tm => tm.Team)
            .FirstOrDefaultAsync(cancellationToken);

        if (userTeam == null || !userTeam.CreatedBy.HasValue)
        {
            return new List<Guid>(); // User not in any team or team has no creator
        }

        // Get teams created by the PM who created the user's team
        var accessibleTeamIds = await _context.Teams
            .Where(t => t.CreatedBy == userTeam.CreatedBy.Value)
            .Select(t => t.TeamId)
            .ToListAsync(cancellationToken);

        return accessibleTeamIds;
    }

    public async Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var accessibleProjectIds = await GetAccessibleProjectIdsAsync(cancellationToken);

        // null means access to all projects
        if (accessibleProjectIds == null)
        {
            return true;
        }

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
}

