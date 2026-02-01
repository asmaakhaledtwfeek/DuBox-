using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, Result<List<PermissionDto>>>
{
    private readonly IDbContext _context;

    public GetRolePermissionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == request.RoleId, cancellationToken);
        if (!roleExists)
        {
            return Result.Failure<List<PermissionDto>>("Role not found");
        }

        var permissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .Select(rp => rp.Permission)
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Module)
            .ThenBy(p => p.Action)
            .Select(p => new PermissionDto(
                p.PermissionId,
                p.Module,
                p.Action,
                p.PermissionKey,
                p.DisplayName,
                p.Description,
                p.Category,
                p.DisplayOrder,
                p.IsActive
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(permissions);
    }
}

