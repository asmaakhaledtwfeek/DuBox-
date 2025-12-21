using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Navigation.Queries;

public class GetNavigationMenuItemsQueryHandler : IRequestHandler<GetNavigationMenuItemsQuery, Result<List<NavigationMenuItemDto>>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetNavigationMenuItemsQueryHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<NavigationMenuItemDto>>> Handle(GetNavigationMenuItemsQuery request, CancellationToken cancellationToken)
    {
        // Get all active menu items
        var menuItems = await _context.NavigationMenuItems
            .Where(m => m.IsActive && m.IsVisible && m.ParentMenuItemId == null)
            .OrderBy(m => m.DisplayOrder)
            .Include(m => m.Children.Where(c => c.IsActive && c.IsVisible))
            .ToListAsync(cancellationToken);

        // Debug: Log all menu items found
        System.Diagnostics.Debug.WriteLine($"🔍 Found {menuItems.Count} menu items in database:");
        foreach (var item in menuItems)
        {
            System.Diagnostics.Debug.WriteLine($"  - {item.Label} ({item.PermissionModule}.{item.PermissionAction})");
        }

        // Get current user's permissions
        var userPermissions = new HashSet<string>();
        
        if (_currentUserService.IsAuthenticated && Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.Group)
                        .ThenInclude(g => g.GroupRoles)
                            .ThenInclude(gr => gr.Role)
                                .ThenInclude(r => r.RolePermissions)
                                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user != null)
            {
                // Get permissions from direct roles
                var directRolePermissions = user.UserRoles
                    .SelectMany(ur => ur.Role.RolePermissions)
                    .Where(rp => rp.Permission.IsActive)
                    .Select(rp => rp.Permission.PermissionKey);

                // Get permissions from group roles
                var groupRolePermissions = user.UserGroups
                    .SelectMany(ug => ug.Group.GroupRoles)
                    .SelectMany(gr => gr.Role.RolePermissions)
                    .Where(rp => rp.Permission.IsActive)
                    .Select(rp => rp.Permission.PermissionKey);

                // Combine all permissions
                userPermissions = directRolePermissions
                    .Union(groupRolePermissions)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // Debug: Log user permissions
                System.Diagnostics.Debug.WriteLine($"🔑 User has {userPermissions.Count} permissions:");
                foreach (var perm in userPermissions.OrderBy(p => p))
                {
                    System.Diagnostics.Debug.WriteLine($"  - {perm}");
                }
            }
        }

        // Filter menu items based on user permissions
        var filteredMenuItems = menuItems
            .Where(m => HasPermission(m.PermissionModule, m.PermissionAction, userPermissions))
            .ToList();

        // Debug: Log filtered menu items
        System.Diagnostics.Debug.WriteLine($"✅ Filtered to {filteredMenuItems.Count} menu items after permission check:");
        foreach (var item in filteredMenuItems)
        {
            System.Diagnostics.Debug.WriteLine($"  - {item.Label} ({item.PermissionModule}.{item.PermissionAction})");
        }

        var result = filteredMenuItems.Select(m => new NavigationMenuItemDto(
            m.MenuItemId,
            m.Label,
            m.Icon,
            m.Route,
            string.IsNullOrEmpty(m.Aliases) ? null : m.Aliases.Split(',', StringSplitOptions.RemoveEmptyEntries),
            m.PermissionModule,
            m.PermissionAction,
            m.DisplayOrder,
            m.IsActive,
            m.Children.Any() ? m.Children
                .Where(c => HasPermission(c.PermissionModule, c.PermissionAction, userPermissions))
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new NavigationMenuItemDto(
                    c.MenuItemId,
                    c.Label,
                    c.Icon,
                    c.Route,
                    string.IsNullOrEmpty(c.Aliases) ? null : c.Aliases.Split(',', StringSplitOptions.RemoveEmptyEntries),
                    c.PermissionModule,
                    c.PermissionAction,
                    c.DisplayOrder,
                    c.IsActive,
                    null
                )).ToList() : null
        )).ToList();

        return Result.Success(result);
    }

    private bool HasPermission(string module, string action, HashSet<string> userPermissions)
    {
        // If no permission requirement, allow access (for public menu items)
        if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action))
        {
            return true;
        }

        // Build permission key in format: "module.action"
        var permissionKey = $"{module.ToLower()}.{action.ToLower()}";
        
        return userPermissions.Contains(permissionKey);
    }
}

