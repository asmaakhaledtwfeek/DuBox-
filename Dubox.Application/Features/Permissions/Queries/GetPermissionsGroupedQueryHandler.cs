using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetPermissionsGroupedQueryHandler : IRequestHandler<GetPermissionsGroupedQuery, Result<List<PermissionGroupDto>>>
{
    private readonly IDbContext _context;

    public GetPermissionsGroupedQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<PermissionGroupDto>>> Handle(GetPermissionsGroupedQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _context.Permissions
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Module)
            .ThenBy(p => p.Action)
            .ToListAsync(cancellationToken);

        var grouped = permissions
            .GroupBy(p => new { p.Category, p.Module })
            .Select(g => new PermissionGroupDto(
                g.Key.Category ?? "Other",
                g.Key.Module,
                g.Select(p => new PermissionDto(
                    p.PermissionId,
                    p.Module,
                    p.Action,
                    p.PermissionKey,
                    p.DisplayName,
                    p.Description,
                    p.Category,
                    p.DisplayOrder,
                    p.IsActive
                )).ToList()
            ))
            .OrderBy(g => g.Category)
            .ThenBy(g => g.Module)
            .ToList();

        return Result.Success(grouped);
    }
}

