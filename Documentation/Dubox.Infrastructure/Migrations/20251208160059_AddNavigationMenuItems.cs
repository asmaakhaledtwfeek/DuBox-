using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavigationMenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Aliases = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermissionModule = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PermissionAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentMenuItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenuItems", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_NavigationMenuItems_NavigationMenuItems_ParentMenuItemId",
                        column: x => x.ParentMenuItemId,
                        principalTable: "NavigationMenuItems",
                        principalColumn: "MenuItemId");
                });

            migrationBuilder.InsertData(
                table: "NavigationMenuItems",
                columns: new[] { "MenuItemId", "Aliases", "CreatedBy", "CreatedDate", "DisplayOrder", "Icon", "IsActive", "IsVisible", "Label", "ModifiedBy", "ModifiedDate", "ParentMenuItemId", "PermissionAction", "PermissionModule", "Route" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0001-000000000001"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "projects", true, true, "Projects", null, null, null, "view", "projects", "/projects" },
                    { new Guid("20000000-0000-0000-0001-000000000002"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, "materials", true, true, "Materials", null, null, null, "view", "materials", "/materials" },
                    { new Guid("20000000-0000-0000-0001-000000000003"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, "location", true, true, "Locations", null, null, null, "view", "locations", "/locations" },
                    { new Guid("20000000-0000-0000-0001-000000000004"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40, "teams", true, true, "Teams", null, null, null, "view", "teams", "/teams" },
                    { new Guid("20000000-0000-0000-0001-000000000005"), "/quality", "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, "qc", true, true, "Quality Control", null, null, null, "view", "wir", "/qc" },
                    { new Guid("20000000-0000-0000-0001-000000000006"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 60, "reports", true, true, "Reports", null, null, null, "view", "reports", "/reports" },
                    { new Guid("20000000-0000-0000-0001-000000000007"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 70, "notifications", true, true, "Notifications", null, null, null, "view", "notifications", "/notifications" },
                    { new Guid("20000000-0000-0000-0001-000000000008"), "/admin/users", "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100, "admin", true, true, "Admin", null, null, null, "view", "users", "/admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenuItems_ParentMenuItemId",
                table: "NavigationMenuItems",
                column: "ParentMenuItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavigationMenuItems");
        }
    }
}
