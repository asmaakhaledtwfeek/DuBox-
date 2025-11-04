namespace Dubox.Application.DTOs;

public record UserRoleDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public List<RoleDto> DirectRoles { get; init; } = new();
    public List<GroupWithRolesDto> Groups { get; init; } = new();
    public List<string> AllRoles { get; init; } = new();
}

public record GroupWithRolesDto
{
    public Guid GroupId { get; init; }
    public string GroupName { get; init; } = string.Empty;
    public List<RoleDto> Roles { get; init; } = new();
}

public record AssignRolesToUserDto
{
    public Guid UserId { get; init; }
    public List<Guid> RoleIds { get; init; } = new();
}

public record AssignUserToGroupsDto
{
    public Guid UserId { get; init; }
    public List<Guid> GroupIds { get; init; } = new();
}

