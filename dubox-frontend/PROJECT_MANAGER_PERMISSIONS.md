# Project Manager Role - Permission Mapping

This document maps the natural language permissions to the technical `module.action` format used in the frontend.

## View Permissions
- View Projects → `projects.view`
- View Boxes → `boxes.view`
- View Activities → `activities.view`
- View Teams → `teams.view`
- View Materials → `materials.view`
- View WIR → `wir.view`
- View Quality Issues → `quality-issues.view`
- View Reports → `reports.view`
- View Users → `users.view`
- View Roles → `roles.view`
- View Groups → `groups.view`
- View Departments → `departments.view`
- View Locations → `locations.view`
- View Dashboard → `dashboard.view`
- View Progress Updates → `progress-updates.view`

## Create Permissions
- Create Projects → `projects.create`
- Create Boxes → `boxes.create`
- Create Activities → `activities.create`
- Create Teams → `teams.create`
- Create Materials → `materials.create`
- Create WIR → `wir.create`
- Create Quality Issues → `quality-issues.create`
- Export Reports → `reports.export`
- Create Users → `users.create`
- Create Locations → `locations.create`
- Export Dashboard → `dashboard.export`
- Create Progress Updates → `progress-updates.create`

## Edit Permissions
- Edit Projects → `projects.edit`
- Edit Boxes → `boxes.edit`
- Edit Activities → `activities.edit`
- Edit Teams → `teams.edit`
- Edit Materials → `materials.edit`
- Approve WIR → `wir.approve`
- Edit Quality Issues → `quality-issues.edit`
- Manage Reports → `reports.manage`
- Edit Users → `users.edit` (basic info only, NO role/group assignment)
- Edit Locations → `locations.edit`
- Edit Progress Updates → `progress-updates.edit`

## Delete & Action Permissions
- Delete Projects → `projects.delete`
- Delete Boxes → `boxes.delete`
- Delete Activities → `activities.delete`
- Reject WIR → `wir.reject`
- Resolve Quality Issues → `quality-issues.resolve`
- Manage Progress Updates → `progress-updates.manage`

## Manage & Advanced Permissions
- Export Projects → `projects.export`
- Update Box Status → `boxes.update-status`
- Assign Team to Activity → `activities.assign-team`
- Manage Team Members → `teams.manage-members`
- Restock Materials → `materials.restock`
- Review WIR → `wir.review`
- Manage Quality Issues → `quality-issues.manage`
- Manage Projects → `projects.manage`
- Import Boxes → `boxes.import`
- Update Activity Progress → `activities.update-progress`
- Manage Teams → `teams.manage`
- Import Materials → `materials.import`
- Manage WIR → `wir.manage`
- Export Boxes → `boxes.export`
- Manage Activities → `activities.manage`
- Manage Boxes → `boxes.manage`

## Summary of Modules with Full Action Set

### Projects
- view, create, edit, delete, manage, export

### Boxes
- view, create, edit, delete, update-status, manage, export, import

### Activities
- view, create, edit, delete, assign-team, update-progress, manage

### Teams
- view, create, edit, manage, manage-members

### Materials
- view, create, edit, restock, import

### WIR
- view, create, approve, reject, review, manage

### Quality Issues
- view, create, edit, resolve, manage

### Reports
- view, export, manage

### Users
- view, create, edit (basic info only)
- **EXCLUDED**: delete, assign-roles, assign-groups

### Roles
- view

### Groups
- view

### Departments
- view

### Locations
- view, create, edit

### Dashboard
- view, export

### Progress Updates
- view, create, edit, manage

