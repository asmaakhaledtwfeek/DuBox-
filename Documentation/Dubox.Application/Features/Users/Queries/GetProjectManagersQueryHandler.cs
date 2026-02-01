using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Users.Queries;

public class GetProjectManagersQueryHandler : IRequestHandler<GetProjectManagersQuery, Result<List<ProjectManagerDto>>>
{
    private readonly IDbContext _context;

    public GetProjectManagersQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ProjectManagerDto>>> Handle(GetProjectManagersQuery request, CancellationToken cancellationToken)
    {
        // Get users who have the "Project Manager" role
        var projectManagers = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.IsActive && 
                   u.UserRoles.Any(ur => ur.Role.RoleName == "ProjectManager" && ur.Role.IsActive))
            .Select(u => new ProjectManagerDto
            {
                UserId = u.UserId,
                FullName = u.FullName ?? u.Email,
                Email = u.Email
            })
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);

        return Result.Success(projectManagers);
    }
}

