# Project Manager Role - Access Restrictions

This document explicitly lists what the Project Manager role **CANNOT** do in the DuBox system.

## ❌ User Management Restrictions

### 1. Cannot Delete Users
- **Permission**: `users.delete` - NOT granted
- **Implementation**: Delete user button hidden in user management table
- **Reason**: User deletion is a critical action reserved for SystemAdmin
- **Component**: `user-management.component.ts`
- **Check**: `canDeleteUser` returns false for ProjectManager

### 2. Cannot Assign Roles to Users
- **Permission**: `users.assign-roles` - NOT granted
- **Implementation**: Role assignment interface hidden/disabled for ProjectManager
- **Reason**: Role assignment affects system-wide permissions and security
- **Component**: `user-management.component.ts`
- **Check**: `canAssignRoles` returns false for ProjectManager
- **Note**: Project Managers can create/edit basic user information (name, email, department) but cannot modify user roles

### 3. Cannot Assign Users to Groups
- **Permission**: `users.assign-groups` - NOT granted
- **Implementation**: Group assignment interface hidden/disabled for ProjectManager
- **Reason**: Group membership affects inherited roles and permissions
- **Component**: `user-management.component.ts`
- **Check**: `canAssignGroups` returns false for ProjectManager
- **Note**: Project Managers can view groups but cannot modify group membership

## ❌ Role Management Restrictions

### 4. Cannot Create Roles
- **Permission**: `roles.create` - NOT granted
- **Implementation**: Create role button hidden
- **Reason**: Role creation is a system administration task
- **Access**: Project Manager can VIEW roles only

### 5. Cannot Edit Roles
- **Permission**: `roles.edit` - NOT granted
- **Implementation**: Edit role button hidden
- **Reason**: Modifying roles affects system security model
- **Access**: Read-only access to roles

### 6. Cannot Delete Roles
- **Permission**: `roles.delete` - NOT granted
- **Implementation**: Delete role button hidden
- **Reason**: Role deletion could break user access across the system
- **Access**: Read-only access to roles

### 7. Cannot Assign Permissions to Roles
- **Permission**: `roles.assign-permissions` - NOT granted
- **Implementation**: Permission assignment interface hidden
- **Reason**: Permission assignment is a critical security function
- **Access**: Cannot modify role-permission mappings

## ❌ Group Management Restrictions

### 8. Cannot Create Groups
- **Permission**: `groups.create` - NOT granted
- **Implementation**: Create group button hidden
- **Reason**: Group creation affects organizational structure
- **Access**: Project Manager can VIEW groups only

### 9. Cannot Edit Groups
- **Permission**: `groups.edit` - NOT granted
- **Implementation**: Edit group button hidden
- **Reason**: Group modification affects inherited permissions
- **Access**: Read-only access to groups

### 10. Cannot Delete Groups
- **Permission**: `groups.delete` - NOT granted
- **Implementation**: Delete group button hidden
- **Reason**: Group deletion could affect multiple users
- **Access**: Read-only access to groups

## ❌ Location Management Restrictions

### 11. Cannot Delete Locations
- **Permission**: `locations.delete` - NOT granted
- **Implementation**: Delete location button hidden
- **Reason**: Location deletion could affect box history and tracking
- **Access**: Project Manager can view/create/edit locations but not delete
- **Note**: Only SystemAdmin can delete locations to prevent data integrity issues

## ❌ System Administration Restrictions

### 12. Cannot Manage System Settings
- **Permission**: `system.manage` - NOT granted
- **Implementation**: System settings menu hidden
- **Reason**: System configuration affects entire application
- **Access**: No access to system settings

### 13. Cannot View Audit Logs (Unless Specifically Granted)
- **Permission**: `audit-logs.view` - May not be granted
- **Implementation**: Audit logs tab may be hidden
- **Reason**: Audit logs contain sensitive security information
- **Access**: Depends on specific deployment requirements

### 14. Cannot Manage Permissions
- **Permission**: `permissions.manage` - NOT granted
- **Implementation**: Permissions management interface hidden
- **Reason**: Permission management is a core security function
- **Access**: Cannot view or modify permission assignments

## Summary of Restricted Actions

| Action | Module | Permission | Granted to PM | Reason for Restriction |
|--------|--------|------------|---------------|------------------------|
| Delete User | users | users.delete | ❌ No | Critical security action |
| Assign Role | users | users.assign-roles | ❌ No | Affects system permissions |
| Assign to Group | users | users.assign-groups | ❌ No | Affects inherited permissions |
| Create Role | roles | roles.create | ❌ No | System administration |
| Edit Role | roles | roles.edit | ❌ No | System administration |
| Delete Role | roles | roles.delete | ❌ No | System administration |
| Assign Permissions | roles | roles.assign-permissions | ❌ No | Critical security function |
| Create Group | groups | groups.create | ❌ No | Organizational structure |
| Edit Group | groups | groups.edit | ❌ No | Organizational structure |
| Delete Group | groups | groups.delete | ❌ No | Organizational structure |
| Delete Location | locations | locations.delete | ❌ No | Data integrity |
| Manage System | system | system.manage | ❌ No | System-wide configuration |
| Manage Permissions | permissions | permissions.manage | ❌ No | Core security |

## What Project Manager CAN Do with Users

Even with restrictions, Project Manager retains important user management capabilities:

✅ **View all users** - See user list and details
✅ **Create new users** - Add users with basic information:
  - Full name
  - Email address
  - Department assignment
  - Initial password
  - Active/inactive status

✅ **Edit user basic information**:
  - Update full name
  - Change department
  - Toggle active/inactive status
  - Reset password (if permitted)

✅ **View user roles and groups** - See what roles/groups a user belongs to (read-only)

## Implementation Verification

### Permission Service
Location: `core/services/permission.service.ts`

```typescript
users: {
  [UserRole.ProjectManager]: ['view', 'create', 'edit'], 
  // NO: 'delete', 'assign-roles', 'assign-groups'
}
```

### Component Checks
Location: `features/admin/user-management/user-management.component.ts`

```typescript
get canDeleteUser(): boolean {
  return this.permissionService.canDelete('users'); // false for PM
}

get canAssignRoles(): boolean {
  return this.permissionService.hasPermission('users', 'assign-roles'); // false for PM
}

get canAssignGroups(): boolean {
  return this.permissionService.hasPermission('users', 'assign-groups'); // false for PM
}
```

### Template Conditional Rendering

**Location 1**: `features/admin/user-management/user-management.component.html`
```html
<!-- Delete button only shown if canDeleteUser -->
<button *ngIf="canDeleteUser" (click)="requestUserDeletion(user)">
  Delete
</button>
```

**Location 2**: `features/admin/user-management/user-details-page/user-details-page.component.html`
```html
<!-- Assign roles section hidden if !canAssignRoles -->
<div class="selection-field" *ngIf="canAssignRoles">
  <div class="selection-info">
    <label>Assign roles</label>
    <!-- Role assignment interface -->
  </div>
  <button (click)="openRolesModal()">Manage</button>
</div>

<!-- Assign groups section hidden if !canAssignGroups -->
<div class="selection-field" *ngIf="canAssignGroups">
  <div class="selection-info">
    <label>Assign groups</label>
    <!-- Group assignment interface -->
  </div>
  <button (click)="openGroupsModal()">Manage</button>
</div>
```

## Testing Verification

When testing as Project Manager:

1. ✅ **Verify delete button NOT visible** on user list (`user-management.component.html`)
2. ✅ **Verify "Assign roles" section NOT visible** in user details page (`user-details-page.component.html`)
3. ✅ **Verify "Assign groups" section NOT visible** in user details page (`user-details-page.component.html`)
4. ✅ **Verify CAN create user** with basic info (name, email, department)
5. ✅ **Verify CAN edit user** basic info (name, email, department, status)
6. ✅ **Verify CAN view existing roles** user has (in "Access overview" section)
7. ✅ **Verify CAN view existing groups** user belongs to (in "Group memberships" section)
8. ✅ **Verify CAN view roles tab** but not create/edit/delete
9. ✅ **Verify CAN view groups tab** but not create/edit/delete

## Security Rationale

These restrictions ensure:

1. **Separation of Duties**: User administration and permission management are separated
2. **Prevent Privilege Escalation**: Project Managers cannot grant themselves additional permissions
3. **Data Integrity**: Critical records (users, roles, locations) are protected from accidental deletion
4. **Audit Trail**: Sensitive operations are limited to SystemAdmin for better accountability
5. **System Stability**: Core system configuration remains under administrator control

## Support

If a Project Manager needs to perform a restricted action:
1. Contact SystemAdmin
2. Submit a request through proper channels
3. SystemAdmin will evaluate and perform the action if approved
4. Action will be logged in audit trail

