using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFactoriesMenuItemAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Factories permissions
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Action", "Category", "CreatedDate", "Description", "DisplayName", "DisplayOrder", "IsActive", "Module", "PermissionKey" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0018-000000000001"), "View", "Factories", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View factory list and details", "View Factories", 170, true, "Factories", "factories.view" },
                    { new Guid("10000000-0000-0000-0018-000000000002"), "Create", "Factories", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create new factories", "Create Factories", 171, true, "Factories", "factories.create" },
                    { new Guid("10000000-0000-0000-0018-000000000003"), "Edit", "Factories", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Edit existing factories", "Edit Factories", 172, true, "Factories", "factories.edit" },
                    { new Guid("10000000-0000-0000-0018-000000000004"), "Delete", "Factories", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delete factories", "Delete Factories", 173, true, "Factories", "factories.delete" },
                    { new Guid("10000000-0000-0000-0018-000000000005"), "Manage", "Factories", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full management of factories", "Manage Factories", 174, true, "Factories", "factories.manage" }
                });

            // Add Factories menu item (insert between Locations and Teams)
            migrationBuilder.InsertData(
                table: "NavigationMenuItems",
                columns: new[] { "MenuItemId", "Aliases", "CreatedBy", "CreatedDate", "DisplayOrder", "Icon", "IsActive", "IsVisible", "Label", "ModifiedBy", "ModifiedDate", "ParentMenuItemId", "PermissionAction", "PermissionModule", "Route" },
                values: new object[] { new Guid("20000000-0000-0000-0001-000000000009"), null, "System", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 35, "factory", true, true, "Factories", null, null, null, "view", "factories", "/factories" });

            // Grant all factories permissions to System Admin role
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "RolePermissionId", "GrantedByUserId", "GrantedDate", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000001000001"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0018-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000001000002"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0018-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000001000003"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0018-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000001000004"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0018-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("20000000-0000-0000-0000-000001000005"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0018-000000000005"), new Guid("11111111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove role permissions
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionId",
                keyValue: new Guid("20000000-0000-0000-0000-000001000001"));
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionId",
                keyValue: new Guid("20000000-0000-0000-0000-000001000002"));
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionId",
                keyValue: new Guid("20000000-0000-0000-0000-000001000003"));
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionId",
                keyValue: new Guid("20000000-0000-0000-0000-000001000004"));
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RolePermissionId",
                keyValue: new Guid("20000000-0000-0000-0000-000001000005"));

            // Remove menu item
            migrationBuilder.DeleteData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000009"));

            // Remove permissions
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("10000000-0000-0000-0018-000000000001"));
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("10000000-0000-0000-0018-000000000002"));
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("10000000-0000-0000-0018-000000000003"));
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("10000000-0000-0000-0018-000000000004"));
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("10000000-0000-0000-0018-000000000005"));
        }
    }
}

