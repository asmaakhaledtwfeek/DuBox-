using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComingSoonToNavigationMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000003"));

            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000007"));

            migrationBuilder.AddColumn<bool>(
                name: "ComingSoon",
                table: "NavigationMenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000001"),
                column: "ComingSoon",
                value: false);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000002"),
                columns: new[] { "ComingSoon", "DisplayOrder" },
                values: new object[] { true, 40 });

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000004"),
                columns: new[] { "ComingSoon", "DisplayOrder" },
                values: new object[] { false, 30 });

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000005"),
                columns: new[] { "ComingSoon", "DisplayOrder" },
                values: new object[] { false, 20 });

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000006"),
                columns: new[] { "ComingSoon", "DisplayOrder" },
                values: new object[] { false, 70 });

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000008"),
                column: "ComingSoon",
                value: false);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000009"),
                columns: new[] { "ComingSoon", "DisplayOrder" },
                values: new object[] { false, 80 });

            migrationBuilder.InsertData(
                table: "NavigationMenuItems",
                columns: new[] { "MenuItemId", "Aliases", "ComingSoon", "CreatedBy", "CreatedDate", "DisplayOrder", "Icon", "IsActive", "IsVisible", "Label", "ModifiedBy", "ModifiedDate", "ParentMenuItemId", "PermissionAction", "PermissionModule", "Route" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0001-000000000010"), null, true, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50, "cost", true, true, "Cost", null, null, null, "view", "cost", "/cost" },
                    { new Guid("20000000-0000-0000-0001-000000000011"), null, true, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 60, "schedule", true, true, "Schedule", null, null, null, "view", "schedule", "/schedule" },
                    { new Guid("20000000-0000-0000-0001-000000000012"), null, true, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 90, "bim", true, true, "BIM", null, null, null, "view", "bim", "/bim" },
                    { new Guid("20000000-0000-0000-0001-000000000013"), null, true, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 110, "help", true, true, "Help", null, null, null, "view", "help", "/help" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000010"));

            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000011"));

            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000012"));

            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000013"));

            migrationBuilder.DropColumn(
                name: "ComingSoon",
                table: "NavigationMenuItems");

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000002"),
                column: "DisplayOrder",
                value: 20);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000004"),
                column: "DisplayOrder",
                value: 40);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000005"),
                column: "DisplayOrder",
                value: 50);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000006"),
                column: "DisplayOrder",
                value: 60);

            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000009"),
                column: "DisplayOrder",
                value: 35);

            migrationBuilder.InsertData(
                table: "NavigationMenuItems",
                columns: new[] { "MenuItemId", "Aliases", "CreatedBy", "CreatedDate", "DisplayOrder", "Icon", "IsActive", "IsVisible", "Label", "ModifiedBy", "ModifiedDate", "ParentMenuItemId", "PermissionAction", "PermissionModule", "Route" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0001-000000000003"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, "location", true, true, "Locations", null, null, null, "view", "locations", "/locations" },
                    { new Guid("20000000-0000-0000-0001-000000000007"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 70, "notifications", true, true, "Notifications", null, null, null, "view", "notifications", "/notifications" }
                });
        }
    }
}
