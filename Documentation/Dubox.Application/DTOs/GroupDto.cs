namespace Dubox.Application.DTOs;

public record GroupDto
{
    public Guid GroupId { get; init; }
    public string GroupName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public List<RoleDto> Roles { get; init; } = new();
}

public record CreateGroupDto
{
    public string GroupName { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public record UpdateGroupDto
{
    public Guid GroupId { get; init; }
    public string GroupName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

public record AssignRolesToGroupDto
{
    public Guid GroupId { get; init; }
    public List<Guid> RoleIds { get; init; } = new();
}

