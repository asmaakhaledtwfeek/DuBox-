namespace Dubox.Application.DTOs;

public record NavigationMenuItemDto(
    Guid MenuItemId,
    string Label,
    string Icon,
    string Route,
    string[]? Aliases,
    string PermissionModule,
    string PermissionAction,
    int DisplayOrder,
    bool IsActive,
    List<NavigationMenuItemDto>? Children
);

