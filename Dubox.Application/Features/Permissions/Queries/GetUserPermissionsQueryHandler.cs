using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, Result<UserPermissionsDto>>
{
    private readonly IDbContext _context;

    public GetUserPermissionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserPermissionsDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserPermissionsDto>("User not found");
        }

        // Get all roles (direct + from groups)
        var directRoles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var groupRoles = user.UserGroups
            .SelectMany(ug => ug.Group.GroupRoles)
            .Select(gr => gr.Role.RoleName)
            .Distinct()
            .ToList();
        var allRoles = directRoles.Union(groupRoles).Distinct().ToList();

        // Get all permissions from all roles
        var directRolePermissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Where(rp => rp.Permission.IsActive)
            .Select(rp => rp.Permission.PermissionKey);

        var groupRolePermissions = user.UserGroups
            .SelectMany(ug => ug.Group.GroupRoles)
            .SelectMany(gr => gr.Role.RolePermissions)
            .Where(rp => rp.Permission.IsActive)
            .Select(rp => rp.Permission.PermissionKey);

        var allPermissionKeys = directRolePermissions
            .Union(groupRolePermissions)
            .Distinct()
            .OrderBy(p => p)
            .ToList();

        var result = new UserPermissionsDto(
            user.UserId,
            user.Email,
            allRoles,
            allPermissionKeys
        );

        return Result.Success(result);
    }
}

