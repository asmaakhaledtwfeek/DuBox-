namespace Dubox.Application.DTOs;

public record RoleDto
{
    public Guid RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
}

public record CreateRoleDto
{
    public string RoleName { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public record UpdateRoleDto
{
    public Guid RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}

