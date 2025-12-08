using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class NavigationMenuSeedData
{
    // Pre-defined Menu Item GUIDs for consistent seeding
    private static readonly Guid ProjectsMenuId = new("20000000-0000-0000-0001-000000000001");
    private static readonly Guid MaterialsMenuId = new("20000000-0000-0000-0001-000000000002");
    private static readonly Guid LocationsMenuId = new("20000000-0000-0000-0001-000000000003");
    private static readonly Guid TeamsMenuId = new("20000000-0000-0000-0001-000000000004");
    private static readonly Guid QualityControlMenuId = new("20000000-0000-0000-0001-000000000005");
    private static readonly Guid ReportsMenuId = new("20000000-0000-0000-0001-000000000006");
    private static readonly Guid NotificationsMenuId = new("20000000-0000-0000-0001-000000000007");
    private static readonly Guid AdminMenuId = new("20000000-0000-0000-0001-000000000008");

    // Static seed date to prevent EF Core model changes warning
    private static readonly DateTime SeedDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedNavigationMenuItems(ModelBuilder modelBuilder)
    {

        var menuItems = new List<NavigationMenuItem>
        {
            new()
            {
                MenuItemId = ProjectsMenuId,
                Label = "Projects",
                Icon = "projects",
                Route = "/projects",
                Aliases = null,
                PermissionModule = "projects",
                PermissionAction = "view",
                DisplayOrder = 10,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = MaterialsMenuId,
                Label = "Materials",
                Icon = "materials",
                Route = "/materials",
                Aliases = null,
                PermissionModule = "materials",
                PermissionAction = "view",
                DisplayOrder = 20,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = LocationsMenuId,
                Label = "Locations",
                Icon = "location",
                Route = "/locations",
                Aliases = null,
                PermissionModule = "locations",
                PermissionAction = "view",
                DisplayOrder = 30,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = TeamsMenuId,
                Label = "Teams",
                Icon = "teams",
                Route = "/teams",
                Aliases = null,
                PermissionModule = "teams",
                PermissionAction = "view",
                DisplayOrder = 40,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = QualityControlMenuId,
                Label = "Quality Control",
                Icon = "qc",
                Route = "/qc",
                Aliases = "/quality",
                PermissionModule = "wir",
                PermissionAction = "view",
                DisplayOrder = 50,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = ReportsMenuId,
                Label = "Reports",
                Icon = "reports",
                Route = "/reports",
                Aliases = null,
                PermissionModule = "reports",
                PermissionAction = "view",
                DisplayOrder = 60,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = NotificationsMenuId,
                Label = "Notifications",
                Icon = "notifications",
                Route = "/notifications",
                Aliases = null,
                PermissionModule = "notifications",
                PermissionAction = "view",
                DisplayOrder = 70,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
            new()
            {
                MenuItemId = AdminMenuId,
                Label = "Admin",
                Icon = "admin",
                Route = "/admin",
                Aliases = "/admin/users",
                PermissionModule = "users",
                PermissionAction = "view",
                DisplayOrder = 100,
                IsActive = true,
                IsVisible = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<NavigationMenuItem>().HasData(menuItems);
    }
}

