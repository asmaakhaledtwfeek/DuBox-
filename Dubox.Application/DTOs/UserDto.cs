namespace Dubox.Application.DTOs;

public record UserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public DateTime CreatedDate { get; init; }
}

public record CreateUserDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
}

public record UpdateUserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
    public bool IsActive { get; init; }
}

