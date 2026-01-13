using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

public static class PermissionSeedData
{
    // Pre-defined Permission GUIDs for consistent seeding
    private static readonly Guid ProjectsViewId = new("10000000-0000-0000-0001-000000000001");
    private static readonly Guid ProjectsCreateId = new("10000000-0000-0000-0001-000000000002");
    private static readonly Guid ProjectsEditId = new("10000000-0000-0000-0001-000000000003");
    private static readonly Guid ProjectsDeleteId = new("10000000-0000-0000-0001-000000000004");
    private static readonly Guid ProjectsExportId = new("10000000-0000-0000-0001-000000000005");
    private static readonly Guid ProjectsManageId = new("10000000-0000-0000-0001-000000000006");

    private static readonly Guid BoxesViewId = new("10000000-0000-0000-0002-000000000001");
    private static readonly Guid BoxesCreateId = new("10000000-0000-0000-0002-000000000002");
    private static readonly Guid BoxesEditId = new("10000000-0000-0000-0002-000000000003");
    private static readonly Guid BoxesDeleteId = new("10000000-0000-0000-0002-000000000004");
    private static readonly Guid BoxesUpdateStatusId = new("10000000-0000-0000-0002-000000000005");
    private static readonly Guid BoxesImportId = new("10000000-0000-0000-0002-000000000006");
    private static readonly Guid BoxesExportId = new("10000000-0000-0000-0002-000000000007");
    private static readonly Guid BoxesManageId = new("10000000-0000-0000-0002-000000000008");

    private static readonly Guid ActivitiesViewId = new("10000000-0000-0000-0003-000000000001");
    private static readonly Guid ActivitiesCreateId = new("10000000-0000-0000-0003-000000000002");
    private static readonly Guid ActivitiesEditId = new("10000000-0000-0000-0003-000000000003");
    private static readonly Guid ActivitiesDeleteId = new("10000000-0000-0000-0003-000000000004");
    private static readonly Guid ActivitiesAssignTeamId = new("10000000-0000-0000-0003-000000000005");
    private static readonly Guid ActivitiesUpdateProgressId = new("10000000-0000-0000-0003-000000000006");
    private static readonly Guid ActivitiesManageId = new("10000000-0000-0000-0003-000000000007");

    private static readonly Guid TeamsViewId = new("10000000-0000-0000-0004-000000000001");
    private static readonly Guid TeamsCreateId = new("10000000-0000-0000-0004-000000000002");
    private static readonly Guid TeamsEditId = new("10000000-0000-0000-0004-000000000003");
    private static readonly Guid TeamsDeleteId = new("10000000-0000-0000-0004-000000000004");
    private static readonly Guid TeamsManageMembersId = new("10000000-0000-0000-0004-000000000005");
    private static readonly Guid TeamsManageId = new("10000000-0000-0000-0004-000000000006");

    private static readonly Guid MaterialsViewId = new("10000000-0000-0000-0005-000000000001");
    private static readonly Guid MaterialsCreateId = new("10000000-0000-0000-0005-000000000002");
    private static readonly Guid MaterialsEditId = new("10000000-0000-0000-0005-000000000003");
    private static readonly Guid MaterialsDeleteId = new("10000000-0000-0000-0005-000000000004");
    private static readonly Guid MaterialsRestockId = new("10000000-0000-0000-0005-000000000005");
    private static readonly Guid MaterialsImportId = new("10000000-0000-0000-0005-000000000006");
    private static readonly Guid MaterialsManageId = new("10000000-0000-0000-0005-000000000007");

    private static readonly Guid WirViewId = new("10000000-0000-0000-0006-000000000001");
    private static readonly Guid WirCreateId = new("10000000-0000-0000-0006-000000000002");
    private static readonly Guid WirApproveId = new("10000000-0000-0000-0006-000000000003");
    private static readonly Guid WirRejectId = new("10000000-0000-0000-0006-000000000004");
    private static readonly Guid WirReviewId = new("10000000-0000-0000-0006-000000000005");
    private static readonly Guid WirManageId = new("10000000-0000-0000-0006-000000000006");

    private static readonly Guid QualityIssuesViewId = new("10000000-0000-0000-0007-000000000001");
    private static readonly Guid QualityIssuesCreateId = new("10000000-0000-0000-0007-000000000002");
    private static readonly Guid QualityIssuesEditId = new("10000000-0000-0000-0007-000000000003");
    private static readonly Guid QualityIssuesResolveId = new("10000000-0000-0000-0007-000000000004");
    private static readonly Guid QualityIssuesManageId = new("10000000-0000-0000-0007-000000000005");

    private static readonly Guid ReportsViewId = new("10000000-0000-0000-0008-000000000001");
    private static readonly Guid ReportsExportId = new("10000000-0000-0000-0008-000000000002");
    private static readonly Guid ReportsManageId = new("10000000-0000-0000-0008-000000000003");

    private static readonly Guid UsersViewId = new("10000000-0000-0000-0009-000000000001");
    private static readonly Guid UsersCreateId = new("10000000-0000-0000-0009-000000000002");
    private static readonly Guid UsersEditId = new("10000000-0000-0000-0009-000000000003");
    private static readonly Guid UsersDeleteId = new("10000000-0000-0000-0009-000000000004");
    private static readonly Guid UsersAssignRolesId = new("10000000-0000-0000-0009-000000000005");
    private static readonly Guid UsersAssignGroupsId = new("10000000-0000-0000-0009-000000000006");
    private static readonly Guid UsersManageId = new("10000000-0000-0000-0009-000000000007");

    private static readonly Guid RolesViewId = new("10000000-0000-0000-0010-000000000001");
    private static readonly Guid RolesCreateId = new("10000000-0000-0000-0010-000000000002");
    private static readonly Guid RolesEditId = new("10000000-0000-0000-0010-000000000003");
    private static readonly Guid RolesDeleteId = new("10000000-0000-0000-0010-000000000004");
    private static readonly Guid RolesAssignPermissionsId = new("10000000-0000-0000-0010-000000000005");
    private static readonly Guid RolesManageId = new("10000000-0000-0000-0010-000000000006");

    private static readonly Guid GroupsViewId = new("10000000-0000-0000-0011-000000000001");
    private static readonly Guid GroupsCreateId = new("10000000-0000-0000-0011-000000000002");
    private static readonly Guid GroupsEditId = new("10000000-0000-0000-0011-000000000003");
    private static readonly Guid GroupsDeleteId = new("10000000-0000-0000-0011-000000000004");
    private static readonly Guid GroupsAssignRolesId = new("10000000-0000-0000-0011-000000000005");
    private static readonly Guid GroupsManageId = new("10000000-0000-0000-0011-000000000006");

    private static readonly Guid DepartmentsViewId = new("10000000-0000-0000-0012-000000000001");
    private static readonly Guid DepartmentsCreateId = new("10000000-0000-0000-0012-000000000002");
    private static readonly Guid DepartmentsEditId = new("10000000-0000-0000-0012-000000000003");
    private static readonly Guid DepartmentsDeleteId = new("10000000-0000-0000-0012-000000000004");
    private static readonly Guid DepartmentsManageId = new("10000000-0000-0000-0012-000000000005");

    private static readonly Guid LocationsViewId = new("10000000-0000-0000-0013-000000000001");
    private static readonly Guid LocationsCreateId = new("10000000-0000-0000-0013-000000000002");
    private static readonly Guid LocationsEditId = new("10000000-0000-0000-0013-000000000003");
    private static readonly Guid LocationsDeleteId = new("10000000-0000-0000-0013-000000000004");
    private static readonly Guid LocationsManageId = new("10000000-0000-0000-0013-000000000005");

    private static readonly Guid DashboardViewId = new("10000000-0000-0000-0014-000000000001");
    private static readonly Guid DashboardExportId = new("10000000-0000-0000-0014-000000000002");

    private static readonly Guid AuditLogsViewId = new("10000000-0000-0000-0015-000000000001");
    private static readonly Guid AuditLogsExportId = new("10000000-0000-0000-0015-000000000002");

    private static readonly Guid ProgressUpdatesViewId = new("10000000-0000-0000-0016-000000000001");
    private static readonly Guid ProgressUpdatesCreateId = new("10000000-0000-0000-0016-000000000002");
    private static readonly Guid ProgressUpdatesEditId = new("10000000-0000-0000-0016-000000000003");
    private static readonly Guid ProgressUpdatesManageId = new("10000000-0000-0000-0016-000000000004");

    private static readonly Guid PermissionsViewId = new("10000000-0000-0000-0017-000000000001");
    private static readonly Guid PermissionsManageId = new("10000000-0000-0000-0017-000000000002");

    // Pre-defined Role GUIDs (must match RoleAndUserSeedData)
    private static readonly Guid SystemAdminRoleId = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ProjectManagerRoleId = new("22222222-2222-2222-2222-222222222222");
    private static readonly Guid SiteEngineerRoleId = new("33333333-3333-3333-3333-333333333333");
    private static readonly Guid ForemanRoleId = new("44444444-4444-4444-4444-444444444444");
    private static readonly Guid QCInspectorRoleId = new("55555555-5555-5555-5555-555555555555");
    private static readonly Guid ProcurementOfficerRoleId = new("66666666-6666-6666-6666-666666666666");
    private static readonly Guid HSEOfficerRoleId = new("77777777-7777-7777-7777-777777777777");
    private static readonly Guid DesignEngineerRoleId = new("88888888-8888-8888-8888-888888888888");
    private static readonly Guid CostEstimatorRoleId = new("99999999-9999-9999-9999-999999999999");
    private static readonly Guid ViewerRoleId = new("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");

    public static void SeedPermissions(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // All permissions
        var permissions = new List<Permission>
        {
            // Projects Module
            new() { PermissionId = ProjectsViewId, Module = "Projects", Action = "View", PermissionKey = "projects.view", DisplayName = "View Projects", Description = "View project list and details", Category = "Projects", DisplayOrder = 1, CreatedDate = now },
            new() { PermissionId = ProjectsCreateId, Module = "Projects", Action = "Create", PermissionKey = "projects.create", DisplayName = "Create Projects", Description = "Create new projects", Category = "Projects", DisplayOrder = 2, CreatedDate = now },
            new() { PermissionId = ProjectsEditId, Module = "Projects", Action = "Edit", PermissionKey = "projects.edit", DisplayName = "Edit Projects", Description = "Edit existing projects", Category = "Projects", DisplayOrder = 3, CreatedDate = now },
            new() { PermissionId = ProjectsDeleteId, Module = "Projects", Action = "Delete", PermissionKey = "projects.delete", DisplayName = "Delete Projects", Description = "Delete projects", Category = "Projects", DisplayOrder = 4, CreatedDate = now },
            new() { PermissionId = ProjectsExportId, Module = "Projects", Action = "Export", PermissionKey = "projects.export", DisplayName = "Export Projects", Description = "Export project data", Category = "Projects", DisplayOrder = 5, CreatedDate = now },
            new() { PermissionId = ProjectsManageId, Module = "Projects", Action = "Manage", PermissionKey = "projects.manage", DisplayName = "Manage Projects", Description = "Full management of projects", Category = "Projects", DisplayOrder = 6, CreatedDate = now },

            // Boxes Module
            new() { PermissionId = BoxesViewId, Module = "Boxes", Action = "View", PermissionKey = "boxes.view", DisplayName = "View Boxes", Description = "View box list and details", Category = "Boxes", DisplayOrder = 10, CreatedDate = now },
            new() { PermissionId = BoxesCreateId, Module = "Boxes", Action = "Create", PermissionKey = "boxes.create", DisplayName = "Create Boxes", Description = "Create new boxes", Category = "Boxes", DisplayOrder = 11, CreatedDate = now },
            new() { PermissionId = BoxesEditId, Module = "Boxes", Action = "Edit", PermissionKey = "boxes.edit", DisplayName = "Edit Boxes", Description = "Edit existing boxes", Category = "Boxes", DisplayOrder = 12, CreatedDate = now },
            new() { PermissionId = BoxesDeleteId, Module = "Boxes", Action = "Delete", PermissionKey = "boxes.delete", DisplayName = "Delete Boxes", Description = "Delete boxes", Category = "Boxes", DisplayOrder = 13, CreatedDate = now },
            new() { PermissionId = BoxesUpdateStatusId, Module = "Boxes", Action = "UpdateStatus", PermissionKey = "boxes.update-status", DisplayName = "Update Box Status", Description = "Update box status", Category = "Boxes", DisplayOrder = 14, CreatedDate = now },
            new() { PermissionId = BoxesImportId, Module = "Boxes", Action = "Import", PermissionKey = "boxes.import", DisplayName = "Import Boxes", Description = "Import boxes from Excel", Category = "Boxes", DisplayOrder = 15, CreatedDate = now },
            new() { PermissionId = BoxesExportId, Module = "Boxes", Action = "Export", PermissionKey = "boxes.export", DisplayName = "Export Boxes", Description = "Export box data", Category = "Boxes", DisplayOrder = 16, CreatedDate = now },
            new() { PermissionId = BoxesManageId, Module = "Boxes", Action = "Manage", PermissionKey = "boxes.manage", DisplayName = "Manage Boxes", Description = "Full management of boxes", Category = "Boxes", DisplayOrder = 17, CreatedDate = now },

            // Activities Module
            new() { PermissionId = ActivitiesViewId, Module = "Activities", Action = "View", PermissionKey = "activities.view", DisplayName = "View Activities", Description = "View activity list and details", Category = "Activities", DisplayOrder = 20, CreatedDate = now },
            new() { PermissionId = ActivitiesCreateId, Module = "Activities", Action = "Create", PermissionKey = "activities.create", DisplayName = "Create Activities", Description = "Create new activities", Category = "Activities", DisplayOrder = 21, CreatedDate = now },
            new() { PermissionId = ActivitiesEditId, Module = "Activities", Action = "Edit", PermissionKey = "activities.edit", DisplayName = "Edit Activities", Description = "Edit existing activities", Category = "Activities", DisplayOrder = 22, CreatedDate = now },
            new() { PermissionId = ActivitiesDeleteId, Module = "Activities", Action = "Delete", PermissionKey = "activities.delete", DisplayName = "Delete Activities", Description = "Delete activities", Category = "Activities", DisplayOrder = 23, CreatedDate = now },
            new() { PermissionId = ActivitiesAssignTeamId, Module = "Activities", Action = "AssignTeam", PermissionKey = "activities.assign-team", DisplayName = "Assign Team to Activity", Description = "Assign teams to activities", Category = "Activities", DisplayOrder = 24, CreatedDate = now },
            new() { PermissionId = ActivitiesUpdateProgressId, Module = "Activities", Action = "UpdateProgress", PermissionKey = "activities.update-progress", DisplayName = "Update Activity Progress", Description = "Update activity progress", Category = "Activities", DisplayOrder = 25, CreatedDate = now },
            new() { PermissionId = ActivitiesManageId, Module = "Activities", Action = "Manage", PermissionKey = "activities.manage", DisplayName = "Manage Activities", Description = "Full management of activities", Category = "Activities", DisplayOrder = 26, CreatedDate = now },

            // Teams Module
            new() { PermissionId = TeamsViewId, Module = "Teams", Action = "View", PermissionKey = "teams.view", DisplayName = "View Teams", Description = "View team list and details", Category = "Teams", DisplayOrder = 30, CreatedDate = now },
            new() { PermissionId = TeamsCreateId, Module = "Teams", Action = "Create", PermissionKey = "teams.create", DisplayName = "Create Teams", Description = "Create new teams", Category = "Teams", DisplayOrder = 31, CreatedDate = now },
            new() { PermissionId = TeamsEditId, Module = "Teams", Action = "Edit", PermissionKey = "teams.edit", DisplayName = "Edit Teams", Description = "Edit existing teams", Category = "Teams", DisplayOrder = 32, CreatedDate = now },
            new() { PermissionId = TeamsDeleteId, Module = "Teams", Action = "Delete", PermissionKey = "teams.delete", DisplayName = "Delete Teams", Description = "Delete teams", Category = "Teams", DisplayOrder = 33, CreatedDate = now },
            new() { PermissionId = TeamsManageMembersId, Module = "Teams", Action = "ManageMembers", PermissionKey = "teams.manage-members", DisplayName = "Manage Team Members", Description = "Add/remove team members", Category = "Teams", DisplayOrder = 34, CreatedDate = now },
            new() { PermissionId = TeamsManageId, Module = "Teams", Action = "Manage", PermissionKey = "teams.manage", DisplayName = "Manage Teams", Description = "Full management of teams", Category = "Teams", DisplayOrder = 35, CreatedDate = now },

            // Materials Module
            new() { PermissionId = MaterialsViewId, Module = "Materials", Action = "View", PermissionKey = "materials.view", DisplayName = "View Materials", Description = "View material list and details", Category = "Materials", DisplayOrder = 40, CreatedDate = now },
            new() { PermissionId = MaterialsCreateId, Module = "Materials", Action = "Create", PermissionKey = "materials.create", DisplayName = "Create Materials", Description = "Create new materials", Category = "Materials", DisplayOrder = 41, CreatedDate = now },
            new() { PermissionId = MaterialsEditId, Module = "Materials", Action = "Edit", PermissionKey = "materials.edit", DisplayName = "Edit Materials", Description = "Edit existing materials", Category = "Materials", DisplayOrder = 42, CreatedDate = now },
            new() { PermissionId = MaterialsDeleteId, Module = "Materials", Action = "Delete", PermissionKey = "materials.delete", DisplayName = "Delete Materials", Description = "Delete materials", Category = "Materials", DisplayOrder = 43, CreatedDate = now },
            new() { PermissionId = MaterialsRestockId, Module = "Materials", Action = "Restock", PermissionKey = "materials.restock", DisplayName = "Restock Materials", Description = "Restock material inventory", Category = "Materials", DisplayOrder = 44, CreatedDate = now },
            new() { PermissionId = MaterialsImportId, Module = "Materials", Action = "Import", PermissionKey = "materials.import", DisplayName = "Import Materials", Description = "Import materials from Excel", Category = "Materials", DisplayOrder = 45, CreatedDate = now },
            new() { PermissionId = MaterialsManageId, Module = "Materials", Action = "Manage", PermissionKey = "materials.manage", DisplayName = "Manage Materials", Description = "Full management of materials", Category = "Materials", DisplayOrder = 46, CreatedDate = now },

            // WIR Module
            new() { PermissionId = WirViewId, Module = "WIR", Action = "View", PermissionKey = "wir.view", DisplayName = "View WIR", Description = "View WIR records and checkpoints", Category = "Quality Control", DisplayOrder = 50, CreatedDate = now },
            new() { PermissionId = WirCreateId, Module = "WIR", Action = "Create", PermissionKey = "wir.create", DisplayName = "Create WIR", Description = "Create new WIR records", Category = "Quality Control", DisplayOrder = 51, CreatedDate = now },
            new() { PermissionId = WirApproveId, Module = "WIR", Action = "Approve", PermissionKey = "wir.approve", DisplayName = "Approve WIR", Description = "Approve WIR records", Category = "Quality Control", DisplayOrder = 52, CreatedDate = now },
            new() { PermissionId = WirRejectId, Module = "WIR", Action = "Reject", PermissionKey = "wir.reject", DisplayName = "Reject WIR", Description = "Reject WIR records", Category = "Quality Control", DisplayOrder = 53, CreatedDate = now },
            new() { PermissionId = WirReviewId, Module = "WIR", Action = "Review", PermissionKey = "wir.review", DisplayName = "Review WIR", Description = "Review WIR checkpoints", Category = "Quality Control", DisplayOrder = 54, CreatedDate = now },
            new() { PermissionId = WirManageId, Module = "WIR", Action = "Manage", PermissionKey = "wir.manage", DisplayName = "Manage WIR", Description = "Full management of WIR", Category = "Quality Control", DisplayOrder = 55, CreatedDate = now },

            // Quality Issues Module
            new() { PermissionId = QualityIssuesViewId, Module = "QualityIssues", Action = "View", PermissionKey = "quality-issues.view", DisplayName = "View Quality Issues", Description = "View quality issues", Category = "Quality Control", DisplayOrder = 60, CreatedDate = now },
            new() { PermissionId = QualityIssuesCreateId, Module = "QualityIssues", Action = "Create", PermissionKey = "quality-issues.create", DisplayName = "Create Quality Issues", Description = "Create quality issues", Category = "Quality Control", DisplayOrder = 61, CreatedDate = now },
            new() { PermissionId = QualityIssuesEditId, Module = "QualityIssues", Action = "Edit", PermissionKey = "quality-issues.edit", DisplayName = "Edit Quality Issues", Description = "Edit quality issues", Category = "Quality Control", DisplayOrder = 62, CreatedDate = now },
            new() { PermissionId = QualityIssuesResolveId, Module = "QualityIssues", Action = "Resolve", PermissionKey = "quality-issues.resolve", DisplayName = "Resolve Quality Issues", Description = "Resolve quality issues", Category = "Quality Control", DisplayOrder = 63, CreatedDate = now },
            new() { PermissionId = QualityIssuesManageId, Module = "QualityIssues", Action = "Manage", PermissionKey = "quality-issues.manage", DisplayName = "Manage Quality Issues", Description = "Full management of quality issues", Category = "Quality Control", DisplayOrder = 64, CreatedDate = now },

            // Reports Module
            new() { PermissionId = ReportsViewId, Module = "Reports", Action = "View", PermissionKey = "reports.view", DisplayName = "View Reports", Description = "View all reports", Category = "Reports", DisplayOrder = 70, CreatedDate = now },
            new() { PermissionId = ReportsExportId, Module = "Reports", Action = "Export", PermissionKey = "reports.export", DisplayName = "Export Reports", Description = "Export report data", Category = "Reports", DisplayOrder = 71, CreatedDate = now },
            new() { PermissionId = ReportsManageId, Module = "Reports", Action = "Manage", PermissionKey = "reports.manage", DisplayName = "Manage Reports", Description = "Full management of reports", Category = "Reports", DisplayOrder = 72, CreatedDate = now },

            // Users Module
            new() { PermissionId = UsersViewId, Module = "Users", Action = "View", PermissionKey = "users.view", DisplayName = "View Users", Description = "View user list and details", Category = "Administration", DisplayOrder = 80, CreatedDate = now },
            new() { PermissionId = UsersCreateId, Module = "Users", Action = "Create", PermissionKey = "users.create", DisplayName = "Create Users", Description = "Create new users", Category = "Administration", DisplayOrder = 81, CreatedDate = now },
            new() { PermissionId = UsersEditId, Module = "Users", Action = "Edit", PermissionKey = "users.edit", DisplayName = "Edit Users", Description = "Edit existing users", Category = "Administration", DisplayOrder = 82, CreatedDate = now },
            new() { PermissionId = UsersDeleteId, Module = "Users", Action = "Delete", PermissionKey = "users.delete", DisplayName = "Delete Users", Description = "Delete users", Category = "Administration", DisplayOrder = 83, CreatedDate = now },
            new() { PermissionId = UsersAssignRolesId, Module = "Users", Action = "AssignRoles", PermissionKey = "users.assign-roles", DisplayName = "Assign User Roles", Description = "Assign roles to users", Category = "Administration", DisplayOrder = 84, CreatedDate = now },
            new() { PermissionId = UsersAssignGroupsId, Module = "Users", Action = "AssignGroups", PermissionKey = "users.assign-groups", DisplayName = "Assign User Groups", Description = "Assign users to groups", Category = "Administration", DisplayOrder = 85, CreatedDate = now },
            new() { PermissionId = UsersManageId, Module = "Users", Action = "Manage", PermissionKey = "users.manage", DisplayName = "Manage Users", Description = "Full management of users", Category = "Administration", DisplayOrder = 86, CreatedDate = now },

            // Roles Module
            new() { PermissionId = RolesViewId, Module = "Roles", Action = "View", PermissionKey = "roles.view", DisplayName = "View Roles", Description = "View role list and details", Category = "Administration", DisplayOrder = 90, CreatedDate = now },
            new() { PermissionId = RolesCreateId, Module = "Roles", Action = "Create", PermissionKey = "roles.create", DisplayName = "Create Roles", Description = "Create new roles", Category = "Administration", DisplayOrder = 91, CreatedDate = now },
            new() { PermissionId = RolesEditId, Module = "Roles", Action = "Edit", PermissionKey = "roles.edit", DisplayName = "Edit Roles", Description = "Edit existing roles", Category = "Administration", DisplayOrder = 92, CreatedDate = now },
            new() { PermissionId = RolesDeleteId, Module = "Roles", Action = "Delete", PermissionKey = "roles.delete", DisplayName = "Delete Roles", Description = "Delete roles", Category = "Administration", DisplayOrder = 93, CreatedDate = now },
            new() { PermissionId = RolesAssignPermissionsId, Module = "Roles", Action = "AssignPermissions", PermissionKey = "roles.assign-permissions", DisplayName = "Assign Role Permissions", Description = "Assign permissions to roles", Category = "Administration", DisplayOrder = 94, CreatedDate = now },
            new() { PermissionId = RolesManageId, Module = "Roles", Action = "Manage", PermissionKey = "roles.manage", DisplayName = "Manage Roles", Description = "Full management of roles", Category = "Administration", DisplayOrder = 95, CreatedDate = now },

            // Groups Module
            new() { PermissionId = GroupsViewId, Module = "Groups", Action = "View", PermissionKey = "groups.view", DisplayName = "View Groups", Description = "View group list and details", Category = "Administration", DisplayOrder = 100, CreatedDate = now },
            new() { PermissionId = GroupsCreateId, Module = "Groups", Action = "Create", PermissionKey = "groups.create", DisplayName = "Create Groups", Description = "Create new groups", Category = "Administration", DisplayOrder = 101, CreatedDate = now },
            new() { PermissionId = GroupsEditId, Module = "Groups", Action = "Edit", PermissionKey = "groups.edit", DisplayName = "Edit Groups", Description = "Edit existing groups", Category = "Administration", DisplayOrder = 102, CreatedDate = now },
            new() { PermissionId = GroupsDeleteId, Module = "Groups", Action = "Delete", PermissionKey = "groups.delete", DisplayName = "Delete Groups", Description = "Delete groups", Category = "Administration", DisplayOrder = 103, CreatedDate = now },
            new() { PermissionId = GroupsAssignRolesId, Module = "Groups", Action = "AssignRoles", PermissionKey = "groups.assign-roles", DisplayName = "Assign Group Roles", Description = "Assign roles to groups", Category = "Administration", DisplayOrder = 104, CreatedDate = now },
            new() { PermissionId = GroupsManageId, Module = "Groups", Action = "Manage", PermissionKey = "groups.manage", DisplayName = "Manage Groups", Description = "Full management of groups", Category = "Administration", DisplayOrder = 105, CreatedDate = now },

            // Departments Module
            new() { PermissionId = DepartmentsViewId, Module = "Departments", Action = "View", PermissionKey = "departments.view", DisplayName = "View Departments", Description = "View department list", Category = "Administration", DisplayOrder = 110, CreatedDate = now },
            new() { PermissionId = DepartmentsCreateId, Module = "Departments", Action = "Create", PermissionKey = "departments.create", DisplayName = "Create Departments", Description = "Create new departments", Category = "Administration", DisplayOrder = 111, CreatedDate = now },
            new() { PermissionId = DepartmentsEditId, Module = "Departments", Action = "Edit", PermissionKey = "departments.edit", DisplayName = "Edit Departments", Description = "Edit existing departments", Category = "Administration", DisplayOrder = 112, CreatedDate = now },
            new() { PermissionId = DepartmentsDeleteId, Module = "Departments", Action = "Delete", PermissionKey = "departments.delete", DisplayName = "Delete Departments", Description = "Delete departments", Category = "Administration", DisplayOrder = 113, CreatedDate = now },
            new() { PermissionId = DepartmentsManageId, Module = "Departments", Action = "Manage", PermissionKey = "departments.manage", DisplayName = "Manage Departments", Description = "Full management of departments", Category = "Administration", DisplayOrder = 114, CreatedDate = now },

            // Locations Module
            new() { PermissionId = LocationsViewId, Module = "Locations", Action = "View", PermissionKey = "locations.view", DisplayName = "View Locations", Description = "View factory locations", Category = "Locations", DisplayOrder = 120, CreatedDate = now },
            new() { PermissionId = LocationsCreateId, Module = "Locations", Action = "Create", PermissionKey = "locations.create", DisplayName = "Create Locations", Description = "Create new locations", Category = "Locations", DisplayOrder = 121, CreatedDate = now },
            new() { PermissionId = LocationsEditId, Module = "Locations", Action = "Edit", PermissionKey = "locations.edit", DisplayName = "Edit Locations", Description = "Edit existing locations", Category = "Locations", DisplayOrder = 122, CreatedDate = now },
            new() { PermissionId = LocationsDeleteId, Module = "Locations", Action = "Delete", PermissionKey = "locations.delete", DisplayName = "Delete Locations", Description = "Delete locations", Category = "Locations", DisplayOrder = 123, CreatedDate = now },
            new() { PermissionId = LocationsManageId, Module = "Locations", Action = "Manage", PermissionKey = "locations.manage", DisplayName = "Manage Locations", Description = "Full management of locations", Category = "Locations", DisplayOrder = 124, CreatedDate = now },

            // Dashboard Module
            new() { PermissionId = DashboardViewId, Module = "Dashboard", Action = "View", PermissionKey = "dashboard.view", DisplayName = "View Dashboard", Description = "View dashboard statistics", Category = "Dashboard", DisplayOrder = 130, CreatedDate = now },
            new() { PermissionId = DashboardExportId, Module = "Dashboard", Action = "Export", PermissionKey = "dashboard.export", DisplayName = "Export Dashboard", Description = "Export dashboard data", Category = "Dashboard", DisplayOrder = 131, CreatedDate = now },

            // Audit Logs Module
            new() { PermissionId = AuditLogsViewId, Module = "AuditLogs", Action = "View", PermissionKey = "audit-logs.view", DisplayName = "View Audit Logs", Description = "View audit logs", Category = "Administration", DisplayOrder = 140, CreatedDate = now },
            new() { PermissionId = AuditLogsExportId, Module = "AuditLogs", Action = "Export", PermissionKey = "audit-logs.export", DisplayName = "Export Audit Logs", Description = "Export audit log data", Category = "Administration", DisplayOrder = 141, CreatedDate = now },

            // Progress Updates Module
            new() { PermissionId = ProgressUpdatesViewId, Module = "ProgressUpdates", Action = "View", PermissionKey = "progress-updates.view", DisplayName = "View Progress Updates", Description = "View progress updates", Category = "Activities", DisplayOrder = 150, CreatedDate = now },
            new() { PermissionId = ProgressUpdatesCreateId, Module = "ProgressUpdates", Action = "Create", PermissionKey = "progress-updates.create", DisplayName = "Create Progress Updates", Description = "Create progress updates", Category = "Activities", DisplayOrder = 151, CreatedDate = now },
            new() { PermissionId = ProgressUpdatesEditId, Module = "ProgressUpdates", Action = "Edit", PermissionKey = "progress-updates.edit", DisplayName = "Edit Progress Updates", Description = "Edit progress updates", Category = "Activities", DisplayOrder = 152, CreatedDate = now },
            new() { PermissionId = ProgressUpdatesManageId, Module = "ProgressUpdates", Action = "Manage", PermissionKey = "progress-updates.manage", DisplayName = "Manage Progress Updates", Description = "Full management of progress updates", Category = "Activities", DisplayOrder = 153, CreatedDate = now },

            // Permissions Module
            new() { PermissionId = PermissionsViewId, Module = "Permissions", Action = "View", PermissionKey = "permissions.view", DisplayName = "View Permissions", Description = "View all permissions", Category = "Administration", DisplayOrder = 160, CreatedDate = now },
            new() { PermissionId = PermissionsManageId, Module = "Permissions", Action = "Manage", PermissionKey = "permissions.manage", DisplayName = "Manage Permissions", Description = "Assign permissions to roles", Category = "Administration", DisplayOrder = 161, CreatedDate = now },
        };

        modelBuilder.Entity<Permission>().HasData(permissions);

        // Now seed the RolePermission relationships
        SeedRolePermissions(modelBuilder);
    }

    private static void SeedRolePermissions(ModelBuilder modelBuilder)
    {
        var rolePermissions = new List<RolePermission>();
        var rpId = 1;

        // Helper to create role permission
        RolePermission CreateRP(Guid roleId, Guid permId) => new()
        {
            RolePermissionId = new Guid($"20000000-0000-0000-0000-{rpId++.ToString().PadLeft(12, '0')}"),
            RoleId = roleId,
            PermissionId = permId,
            GrantedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // System Admin - ALL permissions
        var allPermissionIds = new[] {
            ProjectsViewId, ProjectsCreateId, ProjectsEditId, ProjectsDeleteId, ProjectsExportId, ProjectsManageId,
            BoxesViewId, BoxesCreateId, BoxesEditId, BoxesDeleteId, BoxesUpdateStatusId, BoxesImportId, BoxesExportId, BoxesManageId,
            ActivitiesViewId, ActivitiesCreateId, ActivitiesEditId, ActivitiesDeleteId, ActivitiesAssignTeamId, ActivitiesUpdateProgressId, ActivitiesManageId,
            TeamsViewId, TeamsCreateId, TeamsEditId, TeamsDeleteId, TeamsManageMembersId, TeamsManageId,
            MaterialsViewId, MaterialsCreateId, MaterialsEditId, MaterialsDeleteId, MaterialsRestockId, MaterialsImportId, MaterialsManageId,
            WirViewId, WirCreateId, WirApproveId, WirRejectId, WirReviewId, WirManageId,
            QualityIssuesViewId, QualityIssuesCreateId, QualityIssuesEditId, QualityIssuesResolveId, QualityIssuesManageId,
            ReportsViewId, ReportsExportId, ReportsManageId,
            UsersViewId, UsersCreateId, UsersEditId, UsersDeleteId, UsersAssignRolesId, UsersAssignGroupsId, UsersManageId,
            RolesViewId, RolesCreateId, RolesEditId, RolesDeleteId, RolesAssignPermissionsId, RolesManageId,
            GroupsViewId, GroupsCreateId, GroupsEditId, GroupsDeleteId, GroupsAssignRolesId, GroupsManageId,
            DepartmentsViewId, DepartmentsCreateId, DepartmentsEditId, DepartmentsDeleteId, DepartmentsManageId,
            LocationsViewId, LocationsCreateId, LocationsEditId, LocationsDeleteId, LocationsManageId,
            DashboardViewId, DashboardExportId,
            AuditLogsViewId, AuditLogsExportId,
            ProgressUpdatesViewId, ProgressUpdatesCreateId, ProgressUpdatesEditId, ProgressUpdatesManageId,
            PermissionsViewId, PermissionsManageId
        };

        foreach (var permId in allPermissionIds)
            rolePermissions.Add(CreateRP(SystemAdminRoleId, permId));

        // Project Manager - Most permissions except system admin
        var pmPermissions = new[] {
            ProjectsViewId, ProjectsCreateId, ProjectsEditId, ProjectsDeleteId, ProjectsExportId, ProjectsManageId,
            BoxesViewId, BoxesCreateId, BoxesEditId, BoxesDeleteId, BoxesUpdateStatusId, BoxesImportId, BoxesExportId, BoxesManageId,
            ActivitiesViewId, ActivitiesCreateId, ActivitiesEditId, ActivitiesDeleteId, ActivitiesAssignTeamId, ActivitiesUpdateProgressId, ActivitiesManageId,
            TeamsViewId, TeamsCreateId, TeamsEditId, TeamsManageMembersId, TeamsManageId,
            MaterialsViewId, MaterialsCreateId, MaterialsEditId, MaterialsRestockId, MaterialsImportId,
            WirViewId, WirCreateId, WirApproveId, WirRejectId, WirReviewId, WirManageId,
            QualityIssuesViewId, QualityIssuesCreateId, QualityIssuesEditId, QualityIssuesResolveId, QualityIssuesManageId,
            ReportsViewId, ReportsExportId, ReportsManageId,
            UsersViewId, UsersCreateId, UsersEditId,
            RolesViewId, GroupsViewId,
            DepartmentsViewId,
            LocationsViewId, LocationsCreateId, LocationsEditId,
            DashboardViewId, DashboardExportId,
            ProgressUpdatesViewId, ProgressUpdatesCreateId, ProgressUpdatesEditId, ProgressUpdatesManageId
        };

        foreach (var permId in pmPermissions)
            rolePermissions.Add(CreateRP(ProjectManagerRoleId, permId));

        // Site Engineer
        var sePermissions = new[] {
            ProjectsViewId, ProjectsEditId, ProjectsExportId,
            BoxesViewId, BoxesEditId, BoxesUpdateStatusId, BoxesExportId,
            ActivitiesViewId, ActivitiesCreateId, ActivitiesEditId, ActivitiesAssignTeamId, ActivitiesUpdateProgressId,
            TeamsViewId, TeamsEditId,
            MaterialsViewId,
            WirViewId, WirCreateId, WirApproveId, WirRejectId,
            QualityIssuesViewId, QualityIssuesCreateId,
            ReportsViewId, ReportsExportId,
            DashboardViewId,
            ProgressUpdatesViewId, ProgressUpdatesCreateId, ProgressUpdatesEditId
        };

        foreach (var permId in sePermissions)
            rolePermissions.Add(CreateRP(SiteEngineerRoleId, permId));

        // Foreman
        var foremanPermissions = new[] {
            ProjectsViewId,
            BoxesViewId, BoxesUpdateStatusId,
            ActivitiesViewId, ActivitiesEditId, ActivitiesUpdateProgressId,
            TeamsViewId,
            MaterialsViewId,
            WirViewId,
            QualityIssuesViewId,
            ReportsViewId,
            DashboardViewId,
            ProgressUpdatesViewId, ProgressUpdatesCreateId
        };

        foreach (var permId in foremanPermissions)
            rolePermissions.Add(CreateRP(ForemanRoleId, permId));

        // QC Inspector
        var qcPermissions = new[] {
            ProjectsViewId,
            BoxesViewId, BoxesUpdateStatusId,
            ActivitiesViewId,
            TeamsViewId,
            MaterialsViewId,
            WirViewId, WirCreateId, WirApproveId, WirRejectId, WirReviewId,
            QualityIssuesViewId, QualityIssuesCreateId, QualityIssuesEditId, QualityIssuesResolveId,
            ReportsViewId, ReportsExportId,
            DashboardViewId,
            ProgressUpdatesViewId
        };

        foreach (var permId in qcPermissions)
            rolePermissions.Add(CreateRP(QCInspectorRoleId, permId));


        // Cost Estimator
        var cePermissions = new[] {
            ProjectsViewId, ProjectsExportId,
            BoxesViewId, BoxesExportId,
            ActivitiesViewId,
            TeamsViewId,
            MaterialsViewId, MaterialsCreateId,
            WirViewId,
            QualityIssuesViewId,
            ReportsViewId, ReportsExportId,
            DashboardViewId
        };

        foreach (var permId in cePermissions)
            rolePermissions.Add(CreateRP(CostEstimatorRoleId, permId));

        // Procurement Officer
        var poPermissions = new[] {
            ProjectsViewId,
            BoxesViewId,
            ActivitiesViewId,
            TeamsViewId,
            MaterialsViewId, MaterialsCreateId, MaterialsEditId, MaterialsRestockId, MaterialsImportId,
            WirViewId,
            QualityIssuesViewId,
            ReportsViewId, ReportsExportId,
            DashboardViewId
        };

        foreach (var permId in poPermissions)
            rolePermissions.Add(CreateRP(ProcurementOfficerRoleId, permId));

        // HSE Officer
        var hsePermissions = new[] {
            ProjectsViewId,
            BoxesViewId,
            ActivitiesViewId,
            TeamsViewId,
            MaterialsViewId,
            WirViewId, WirApproveId,
            QualityIssuesViewId, QualityIssuesCreateId,
            ReportsViewId, ReportsExportId,
            DashboardViewId
        };

        foreach (var permId in hsePermissions)
            rolePermissions.Add(CreateRP(HSEOfficerRoleId, permId));

        // Viewer - Only view permissions
        var viewerPermissions = new[] {
            ProjectsViewId,
            BoxesViewId,
            ActivitiesViewId,
            TeamsViewId,
            MaterialsViewId,
            WirViewId,
            QualityIssuesViewId,
            ReportsViewId,
            DashboardViewId,
            ProgressUpdatesViewId
        };

        foreach (var permId in viewerPermissions)
            rolePermissions.Add(CreateRP(ViewerRoleId, permId));

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }
}

