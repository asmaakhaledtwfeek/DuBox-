using Dubox.Domain.Entities;
using Dubox.Domain.Services;

namespace Dubox.Infrastructure.Services;

public class UserRolePermissionService : IUserRolePermissionService
{
    public List<string> GetAllUserRoles(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var directRoles = GetDirectUserRoles(user);
        
        var groupRoles = user.UserGroups
            .Where(ug => ug.Group != null)
            .SelectMany(ug => ug.Group!.GroupRoles)
            .Where(gr => gr.Role != null)
            .Select(gr => gr.Role!.RoleName)
            .Distinct()
            .ToList();

        return directRoles
            .Union(groupRoles)
            .Distinct()
            .ToList();
    }
    public List<string> GetDirectUserRoles(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return user.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => ur.Role!.RoleName)
            .ToList();
    }
    public List<string> GetAllUserPermissionKeys(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        // Get permissions from direct roles
        var directRolePermissions = user.UserRoles
            .Where(ur => ur.Role != null)
            .SelectMany(ur => ur.Role!.RolePermissions)
            .Where(rp => rp.Permission != null && rp.Permission.IsActive)
            .Select(rp => rp.Permission!.PermissionKey);

        // Get permissions from group roles
        var groupRolePermissions = user.UserGroups
            .Where(ug => ug.Group != null)
            .SelectMany(ug => ug.Group!.GroupRoles)
            .Where(gr => gr.Role != null)
            .SelectMany(gr => gr.Role!.RolePermissions)
            .Where(rp => rp.Permission != null && rp.Permission.IsActive)
            .Select(rp => rp.Permission!.PermissionKey);

        // Combine, deduplicate, and sort
        return directRolePermissions
            .Union(groupRolePermissions)
            .Distinct()
            .OrderBy(p => p)
            .ToList();
    }
}
