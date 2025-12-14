using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000008"),
                column: "PermissionAction",
                value: "view");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NavigationMenuItems",
                keyColumn: "MenuItemId",
                keyValue: new Guid("20000000-0000-0000-0001-000000000008"),
                column: "PermissionAction",
                value: "manage");
        }
    }
}
