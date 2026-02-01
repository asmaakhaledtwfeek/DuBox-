using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminMenuPermissionToManage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update Admin menu item to require 'manage' permission instead of 'view'
            // This ensures only SystemAdmin role can see the Admin menu item
            migrationBuilder.Sql(@"
                UPDATE NavigationMenuItems 
                SET PermissionAction = 'manage' 
                WHERE MenuItemId = '20000000-0000-0000-0001-000000000008' 
                AND PermissionModule = 'users'
                AND Label = 'Admin'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Admin menu item back to 'view' permission
            migrationBuilder.Sql(@"
                UPDATE NavigationMenuItems 
                SET PermissionAction = 'view' 
                WHERE MenuItemId = '20000000-0000-0000-0001-000000000008' 
                AND PermissionModule = 'users'
                AND Label = 'Admin'
            ");
        }
    }
}

