using Dubox.Application.Abstractions;
using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IDbContext context, 
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Load user with all relationships for roles and groups
        var user = await _context.Users
            .Include(u => u.EmployeeOfDepartment)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.Group)
                    .ThenInclude(g => g.GroupRoles)
                        .ThenInclude(gr => gr.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        if (!user.IsActive)
            return Result.Failure<LoginResponseDto>("User account is inactive");

        if (string.IsNullOrEmpty(user.PasswordHash) || 
            !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
            return Result.Failure<LoginResponseDto>("Invalid email or password");

        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtProvider.GenerateToken(user);

        // Extract direct roles
        var directRoles = user.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => ur.Role!.RoleName)
            .ToList();

        // Extract groups with their roles
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

        // Combine all roles (direct + from groups) - distinct by role name
        var allRoles = directRoles
            .Concat(groups.SelectMany(g => g.Roles.Select(r => r.RoleName)))
            .Distinct()
            .ToList();

        // Create user DTO with roles and groups
        var userDto = new UserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            DepartmentId = user.DepartmentId,
            Department = user.EmployeeOfDepartment?.DepartmentName,
            IsActive = user.IsActive,
            LastLoginDate = user.LastLoginDate,
            CreatedDate = user.CreatedDate,
            DirectRoles = directRoles,
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

