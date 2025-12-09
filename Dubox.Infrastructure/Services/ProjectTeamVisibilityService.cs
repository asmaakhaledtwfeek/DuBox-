using Dubox.Domain.Abstraction;
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
    private const string ViewerRole = "Viewer";

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

        // SystemAdmin and ProjectManager can create projects/teams
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

        // Check if user is SystemAdmin or Viewer (both have full visibility)
        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);

        if (isSystemAdmin || isViewer)
        {
            return null; // null means access to ALL projects
        }

        // Check if user is ProjectManager
        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            // PM sees: own projects + System Admin projects
            var pmOwnProjects = await _context.Projects
                .Where(p => p.CreatedBy == userId.ToString())
                .Select(p => p.ProjectId)
                .ToListAsync(cancellationToken);

            // Add System Admin projects if user is in their teams
            var systemAdminProjects = await GetSystemAdminProjectsForUserAsync(userId, cancellationToken);

            // Combine all lists
            var pmAccessibleProjects = pmOwnProjects.Union(systemAdminProjects).Distinct().ToList();
            return pmAccessibleProjects;
        }

        // For all other roles:
        // Get own projects + projects created by ANY team creator the user belongs to
        var ownProjects = await _context.Projects
            .Where(p => p.CreatedBy == userId.ToString())
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        // Get projects from ALL team creators (PM, SA, or anyone who created teams the user belongs to)
        var teamCreatorProjects = await GetAllTeamCreatorProjectsForUserAsync(userId, cancellationToken);

        // Combine own projects with team creator projects
        var allAccessibleProjects = ownProjects.Union(teamCreatorProjects).Distinct().ToList();
        return allAccessibleProjects;
    }

    /// <summary>
    /// Gets ALL projects created by ANY creator of teams the user belongs to
    /// This includes System Admins, Project Managers, and any other role that created a team
    /// </summary>
    private async Task<List<Guid>> GetAllTeamCreatorProjectsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Find all teams the user belongs to
        var userTeams = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.Team)
            .ToListAsync(cancellationToken);

        if (!userTeams.Any())
        {
            return new List<Guid>();
        }

        // Get all unique creator IDs from these teams
        var teamCreatorIds = userTeams
            .Where(t => t.CreatedBy.HasValue)
            .Select(t => t.CreatedBy!.Value)
            .Distinct()
            .ToList();

        if (!teamCreatorIds.Any())
        {
            return new List<Guid>();
        }

        // Get all projects created by any of these team creators
        var creatorIdStrings = teamCreatorIds.Select(id => id.ToString()).ToList();
        var projectIds = await _context.Projects
            .Where(p => creatorIdStrings.Contains(p.CreatedBy))
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        return projectIds;
    }

    /// <summary>
    /// Gets projects created by System Admins for users who belong to teams created by those System Admins
    /// DEPRECATED: Use GetAllTeamCreatorProjectsForUserAsync instead for comprehensive coverage
    /// Kept for backward compatibility with Project Manager logic
    /// </summary>
    private async Task<List<Guid>> GetSystemAdminProjectsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Find all teams the user belongs to
        var userTeams = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.Team)
            .ToListAsync(cancellationToken);

        if (!userTeams.Any())
        {
            return new List<Guid>();
        }

        // Get unique System Admin IDs who created these teams
        var systemAdminIds = new List<Guid>();
        foreach (var team in userTeams)
        {
            if (team.CreatedBy.HasValue)
            {
                // Check if the team creator is a System Admin
                var isSystemAdmin = await _userRoleService.UserHasRoleAsync(team.CreatedBy.Value, SystemAdminRole, cancellationToken);
                if (isSystemAdmin)
                {
                    systemAdminIds.Add(team.CreatedBy.Value);
                }
            }
        }

        if (!systemAdminIds.Any())
        {
            return new List<Guid>();
        }

        // Get all projects created by those System Admins
        var systemAdminCreatedByStrings = systemAdminIds.Select(id => id.ToString()).ToList();
        var projectIds = await _context.Projects
            .Where(p => systemAdminCreatedByStrings.Contains(p.CreatedBy))
            .Select(p => p.ProjectId)
            .ToListAsync(cancellationToken);

        return projectIds;
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

        // Check if user is SystemAdmin or Viewer (both have full visibility)
        var isSystemAdmin = await _userRoleService.UserHasRoleAsync(userId, SystemAdminRole, cancellationToken);
        var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);

        if (isSystemAdmin || isViewer)
        {
            return null; // null means access to ALL teams
        }

        // Check if user is ProjectManager
        var isProjectManager = await _userRoleService.UserHasRoleAsync(userId, ProjectManagerRole, cancellationToken);
        if (isProjectManager)
        {
            // PM sees: teams they created + teams they are members of
            var createdTeams = await _context.Teams
                .Where(t => t.CreatedBy == userId)
                .Select(t => t.TeamId)
                .ToListAsync(cancellationToken);

            var memberTeams = await _context.TeamMembers
                .Where(tm => tm.UserId == userId && tm.IsActive)
                .Select(tm => tm.TeamId)
                .ToListAsync(cancellationToken);

            // Combine and return unique team IDs
            var allPMTeams = createdTeams.Union(memberTeams).Distinct().ToList();
            return allPMTeams;
        }

        // For all other users (Site Engineers, etc.):
        // Return ONLY teams where the user is a direct member
        var memberTeamIds = await _context.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .Select(tm => tm.TeamId)
            .ToListAsync(cancellationToken);

        return memberTeamIds;
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

    public async Task<bool> CanModifyDataAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return false;
        }

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return false;
        }

        // Check if user is Viewer (read-only role)
        var isViewer = await _userRoleService.UserHasRoleAsync(userId, ViewerRole, cancellationToken);

        // Viewer cannot modify data
        return !isViewer;
    }
}

