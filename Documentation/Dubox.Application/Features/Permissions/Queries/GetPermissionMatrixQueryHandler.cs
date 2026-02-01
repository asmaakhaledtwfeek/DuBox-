using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetPermissionMatrixQueryHandler : IRequestHandler<GetPermissionMatrixQuery, Result<List<RolePermissionMatrixDto>>>
{
    private readonly IDbContext _context;

    public GetPermissionMatrixQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<RolePermissionMatrixDto>>> Handle(GetPermissionMatrixQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .Where(r => r.IsActive)
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .OrderBy(r => r.RoleName)
            .ToListAsync(cancellationToken);

        var matrix = roles.Select(r => new RolePermissionMatrixDto(
            r.RoleId,
            r.RoleName,
            r.Description,
            r.RolePermissions
                .Where(rp => rp.Permission.IsActive)
                .Select(rp => rp.Permission.PermissionKey)
                .ToList()
        )).ToList();

        return Result.Success(matrix);
    }
}

