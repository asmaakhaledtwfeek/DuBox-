using Dubox.Domain.Services;
using Dubox.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IDbContext _context;

    public UserRoleService(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var directRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.RoleName)
            .ToListAsync(cancellationToken);

        var groupRoles = await _context.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupRoles)
            .Select(gr => gr.Role.RoleName)
            .ToListAsync(cancellationToken);

        return directRoles.Concat(groupRoles).Distinct();
    }

    public async Task<bool> UserHasRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken = default)
    {
        var hasDirectRole = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.Role.RoleName == roleName, cancellationToken);

        if (hasDirectRole)
            return true;

        var hasGroupRole = await _context.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupRoles)
            .AnyAsync(gr => gr.Role.RoleName == roleName, cancellationToken);

        return hasGroupRole;
    }

    public async Task<bool> UserHasAnyRoleAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
    {
        var roles = roleNames.ToList();

        var hasDirectRole = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && roles.Contains(ur.Role.RoleName), cancellationToken);

        if (hasDirectRole)
            return true;

        var hasGroupRole = await _context.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupRoles)
            .AnyAsync(gr => roles.Contains(gr.Role.RoleName), cancellationToken);

        return hasGroupRole;
    }
}

