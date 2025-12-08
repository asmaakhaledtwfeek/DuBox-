namespace Dubox.Application.DTOs;

public record PermissionDto(
    Guid PermissionId,
    string Module,
    string Action,
    string PermissionKey,
    string? DisplayName,
    string? Description,
    string? Category,
    int DisplayOrder,
    bool IsActive
);

public record PermissionGroupDto(
    string Category,
    string Module,
    List<PermissionDto> Permissions
);

public record RolePermissionDto(
    Guid RolePermissionId,
    Guid RoleId,
    string RoleName,
    Guid PermissionId,
    string PermissionKey,
    string? DisplayName,
    DateTime GrantedDate
);

public record RolePermissionMatrixDto(
    Guid RoleId,
    string RoleName,
    string? RoleDescription,
    List<string> PermissionKeys
);

public record AssignPermissionsToRoleRequest(
    Guid RoleId,
    List<Guid> PermissionIds
);

public record UserPermissionsDto(
    Guid UserId,
    string Email,
    List<string> AllRoles,
    List<string> PermissionKeys
);

