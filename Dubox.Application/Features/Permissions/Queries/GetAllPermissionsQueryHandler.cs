using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionDto>>>
{
    private readonly IDbContext _context;

    public GetAllPermissionsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _context.Permissions
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

