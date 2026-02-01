using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Permissions.Queries;

public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, Result<UserPermissionsDto>>
{
    private readonly IDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRolePermissionService _userRolePermissionService;

    public GetUserPermissionsQueryHandler(
        IDbContext context, 
        IUnitOfWork unitOfWork,
        IUserRolePermissionService userRolePermissionService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _userRolePermissionService = userRolePermissionService;
    }

    public async Task<Result<UserPermissionsDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var user = _unitOfWork.Repository<User>().GetEntityWithSpec(new GetUserPermissionSpecification(request.UserId));
        if (user == null)
            return Result.Failure<UserPermissionsDto>("User not found");

        var allRoles = _userRolePermissionService.GetAllUserRoles(user);

        var allPermissionKeys = _userRolePermissionService.GetAllUserPermissionKeys(user);

        var result = new UserPermissionsDto(
            user.UserId,
            user.Email,
            allRoles,
            allPermissionKeys
        );

        return Result.Success(result);
    }
}

