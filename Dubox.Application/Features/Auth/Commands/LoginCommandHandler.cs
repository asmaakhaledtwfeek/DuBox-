using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRolePermissionService _userRolePermissionService;

    public LoginCommandHandler(
        IDbContext context, 
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider, 
        IUnitOfWork unitOfWork,
        IUserRolePermissionService userRolePermissionService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
        _userRolePermissionService = userRolePermissionService;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = _unitOfWork.Repository<User>().GetEntityWithSpec(new GetUserWithRolesSpecification(request.Email));
        if (user == null)
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        if (!user.IsActive)
            return Result.Failure<LoginResponseDto>("User account is inactive");

        if (string.IsNullOrEmpty(user.PasswordHash) || 
            !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        user.LastLoginDate = DateTime.UtcNow;
         _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.CompleteAsync();

        var token = _jwtProvider.GenerateToken(user);

        var directRoles = _userRolePermissionService.GetDirectUserRoles(user);

        var groups = user.UserGroups
            .Where(ug => ug.Group != null)
            .Select(ug => new GroupWithRolesDto
            {
                GroupId = ug.Group!.GroupId,
                GroupName = ug.Group.GroupName,
                Roles = ug.Group.GroupRoles
                    .Where(gr => gr.Role != null)
                    .Select(gr => new RoleDto
                    {
                        RoleId = gr.Role!.RoleId,
                        RoleName = gr.Role.RoleName,
                        Description = gr.Role.Description,
                        IsActive = gr.Role.IsActive,
                        CreatedDate = gr.Role.CreatedDate
                    })
                    .ToList()
            })
            .ToList();

        var allRoles = _userRolePermissionService.GetAllUserRoles(user);

        var userDto = user.Adapt<UserDto>() with
        {
            DirectRoles= directRoles,
            Groups = groups,
            AllRoles = allRoles
        };

        var response = new LoginResponseDto
        {
            Token = token,
            RefreshToken = token, // TODO: Implement refresh token if needed
            ExpiresIn = 3600, // 1 hour
            User = userDto
        };

        return Result.Success(response);
    }
}

