using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Groups.Queries;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, Result<List<GroupDto>>>
{
    private readonly IDbContext _context;

    public GetAllGroupsQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<GroupDto>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _context.Groups
            .Include(g => g.GroupRoles)
                .ThenInclude(gr => gr.Role)
            .ToListAsync(cancellationToken);

        var groupDtos = groups.Select(g => new GroupDto
        {
            GroupId = g.GroupId,
            GroupName = g.GroupName,
            Description = g.Description,
            IsActive = g.IsActive,
            CreatedDate = g.CreatedDate,
            Roles = g.GroupRoles.Select(gr => gr.Role.Adapt<RoleDto>()).ToList()
        }).ToList();

        return Result.Success(groupDtos);
    }
}

