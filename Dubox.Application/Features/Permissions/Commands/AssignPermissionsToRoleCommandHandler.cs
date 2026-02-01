using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Commands;

public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, Result<bool>>
{
    private readonly IDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AssignPermissionsToRoleCommandHandler(IDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == request.RoleId, cancellationToken);
        if (role == null)
        {
            return Result.Failure<bool>("Role not found");
        }

        // Remove existing role permissions
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .ToListAsync(cancellationToken);

        _context.RolePermissions.RemoveRange(existingPermissions);

        // Get current user ID for audit trail
        Guid? grantedByUserId = null;
        if (Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            grantedByUserId = userId;
        }

        // Add new permissions
        if (request.PermissionIds.Any())
        {
            // Validate that all permission IDs exist
            var validPermissionIds = await _context.Permissions
                .Where(p => request.PermissionIds.Contains(p.PermissionId) && p.IsActive)
                .Select(p => p.PermissionId)
                .ToListAsync(cancellationToken);

            var newRolePermissions = validPermissionIds.Select(permId => new RolePermission
            {
                RoleId = request.RoleId,
                PermissionId = permId,
                GrantedDate = DateTime.UtcNow,
                GrantedByUserId = grantedByUserId
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(newRolePermissions, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true, "Permissions assigned successfully");
    }
}

