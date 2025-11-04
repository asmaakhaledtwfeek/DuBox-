using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Users.Queries;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<UserRoleDto>>
{
    private readonly IDbContext _context;

    public GetUserRolesQueryHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.Group)
                    .ThenInclude(g => g.GroupRoles)
                        .ThenInclude(gr => gr.Role)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
            return Result.Failure<UserRoleDto>("User not found");

        var directRoles = user.UserRoles
            .Select(ur => ur.Role.Adapt<RoleDto>())
            .ToList();

        var groups = user.UserGroups
            .Select(ug => new GroupWithRolesDto
            {
                GroupId = ug.Group.GroupId,
                GroupName = ug.Group.GroupName,
                Roles = ug.Group.GroupRoles
                    .Select(gr => gr.Role.Adapt<RoleDto>())
                    .ToList()
            })
            .ToList();

        var allRoles = directRoles
            .Select(r => r.RoleName)
            .Concat(groups.SelectMany(g => g.Roles.Select(r => r.RoleName)))
            .Distinct()
            .ToList();

        var userRoleDto = new UserRoleDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            DirectRoles = directRoles,
            Groups = groups,
            AllRoles = allRoles
        };

        return Result.Success(userRoleDto);
    }
}

