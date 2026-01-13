namespace Dubox.Application.DTOs;

public class PaginatedUsersResponseDto
{
    public List<UserDto> Items { get; set; } = new List<UserDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public record UserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
    public string? Department { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public DateTime CreatedDate { get; init; }
    
    // Role and Group information for frontend
    public List<string> DirectRoles { get; init; } = new();
    public List<GroupWithRolesDto> Groups { get; init; } = new();
    public List<string> AllRoles { get; init; } = new();
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

