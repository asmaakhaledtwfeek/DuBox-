using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDuplicatePermissionsIdempotent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Idempotent SQL - only insert permissions if they don't exist
            migrationBuilder.Sql(@"
                -- Materials View Permission
                IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionKey = 'materials.view')
                BEGIN
                    INSERT INTO Permissions (PermissionId, Module, Action, PermissionKey, DisplayName, Description, Category, DisplayOrder, IsActive, CreatedDate)
                    VALUES ('10000000-0000-0000-0016-000000000001', 'Materials', 'View', 'materials.view', 'View Materials', 'View materials inventory and catalog', 'Coming Soon', 160, 1, GETUTCDATE())
                END

                -- Cost View Permission
                IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionKey = 'cost.view')
                BEGIN
                    INSERT INTO Permissions (PermissionId, Module, Action, PermissionKey, DisplayName, Description, Category, DisplayOrder, IsActive, CreatedDate)
                    VALUES ('10000000-0000-0000-0018-000000000001', 'Cost', 'View', 'cost.view', 'View Cost', 'View cost management features', 'Coming Soon', 170, 1, GETUTCDATE())
                END

                -- Schedule View Permission
                IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionKey = 'schedule.view')
                BEGIN
                    INSERT INTO Permissions (PermissionId, Module, Action, PermissionKey, DisplayName, Description, Category, DisplayOrder, IsActive, CreatedDate)
                    VALUES ('10000000-0000-0000-0019-000000000001', 'Schedule', 'View', 'schedule.view', 'View Schedule', 'View schedule management features', 'Coming Soon', 171, 1, GETUTCDATE())
                END

                -- BIM View Permission
                IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionKey = 'bim.view')
                BEGIN
                    INSERT INTO Permissions (PermissionId, Module, Action, PermissionKey, DisplayName, Description, Category, DisplayOrder, IsActive, CreatedDate)
                    VALUES ('10000000-0000-0000-0020-000000000001', 'BIM', 'View', 'bim.view', 'View BIM', 'View BIM features', 'Coming Soon', 172, 1, GETUTCDATE())
                END

                -- Help View Permission
                IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionKey = 'help.view')
                BEGIN
                    INSERT INTO Permissions (PermissionId, Module, Action, PermissionKey, DisplayName, Description, Category, DisplayOrder, IsActive, CreatedDate)
                    VALUES ('10000000-0000-0000-0021-000000000001', 'Help', 'View', 'help.view', 'View Help', 'Access help and documentation', 'General', 173, 1, GETUTCDATE())
                END
            ");

            // Idempotent SQL - only insert role permissions if they don't exist
            // Use actual PermissionIds from database instead of hardcoded IDs
            migrationBuilder.Sql(@"
                -- Declare variables to hold permission IDs
                DECLARE @MaterialsPermId UNIQUEIDENTIFIER = (SELECT PermissionId FROM Permissions WHERE PermissionKey = 'materials.view')
                DECLARE @CostPermId UNIQUEIDENTIFIER = (SELECT PermissionId FROM Permissions WHERE PermissionKey = 'cost.view')
                DECLARE @SchedulePermId UNIQUEIDENTIFIER = (SELECT PermissionId FROM Permissions WHERE PermissionKey = 'schedule.view')
                DECLARE @BIMPermId UNIQUEIDENTIFIER = (SELECT PermissionId FROM Permissions WHERE PermissionKey = 'bim.view')
                DECLARE @HelpPermId UNIQUEIDENTIFIER = (SELECT PermissionId FROM Permissions WHERE PermissionKey = 'help.view')

                -- System Admin Role Permissions (RoleId: 11111111-1111-1111-1111-111111111111)
                IF @MaterialsPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '11111111-1111-1111-1111-111111111111' AND PermissionId = @MaterialsPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '11111111-1111-1111-1111-111111111111', @MaterialsPermId, GETUTCDATE())

                IF @CostPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '11111111-1111-1111-1111-111111111111' AND PermissionId = @CostPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '11111111-1111-1111-1111-111111111111', @CostPermId, GETUTCDATE())

                IF @SchedulePermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '11111111-1111-1111-1111-111111111111' AND PermissionId = @SchedulePermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '11111111-1111-1111-1111-111111111111', @SchedulePermId, GETUTCDATE())

                IF @BIMPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '11111111-1111-1111-1111-111111111111' AND PermissionId = @BIMPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '11111111-1111-1111-1111-111111111111', @BIMPermId, GETUTCDATE())

                IF @HelpPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '11111111-1111-1111-1111-111111111111' AND PermissionId = @HelpPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '11111111-1111-1111-1111-111111111111', @HelpPermId, GETUTCDATE())

                -- Project Manager Role Permissions (RoleId: 22222222-2222-2222-2222-222222222222)
                IF @MaterialsPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '22222222-2222-2222-2222-222222222222' AND PermissionId = @MaterialsPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '22222222-2222-2222-2222-222222222222', @MaterialsPermId, GETUTCDATE())

                IF @CostPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '22222222-2222-2222-2222-222222222222' AND PermissionId = @CostPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '22222222-2222-2222-2222-222222222222', @CostPermId, GETUTCDATE())

                IF @SchedulePermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '22222222-2222-2222-2222-222222222222' AND PermissionId = @SchedulePermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '22222222-2222-2222-2222-222222222222', @SchedulePermId, GETUTCDATE())

                IF @BIMPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '22222222-2222-2222-2222-222222222222' AND PermissionId = @BIMPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '22222222-2222-2222-2222-222222222222', @BIMPermId, GETUTCDATE())

                IF @HelpPermId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = '22222222-2222-2222-2222-222222222222' AND PermissionId = @HelpPermId)
                    INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, GrantedDate)
                    VALUES (NEWID(), '22222222-2222-2222-2222-222222222222', @HelpPermId, GETUTCDATE())
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
