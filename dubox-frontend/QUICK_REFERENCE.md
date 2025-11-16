# 🚀 DuBox Angular Frontend - Quick Reference

## ✅ What's Been Updated

### 1. Role & Permission System ✨ **NEW**
- **10 Roles** from Group AMANA implemented
- **Role Inheritance** from groups supported
- **Granular Permissions** per module and action
- **Multi-role Support** (users can have multiple roles)

### 2. Updated Models
- `User` model now includes: `directRoles`, `groups`, `allRoles`
- All 10 Group AMANA roles defined in `UserRole` enum
- Helper functions for role checking

### 3. Updated Services
- `AuthService` - Now handles role inheritance
- `PermissionService` - Complete permission matrix for all roles
- Module permissions defined for all 10 roles

### 4. Updated Components
- Header displays primary role and department
- Sidebar shows menu items based on user permissions
- Role-based navigation

---

## 🔐 Test Users (Default Password: `AMANA@2024`)

### System Administrator
```
Email: admin@groupamana.com
Roles: SystemAdmin, ProjectManager
Access: Full system access
```

### Management
```
Email: ahmed.almazrouei@groupamana.com
Roles: SystemAdmin, ProjectManager, CostEstimator
Department: Management
```

### Engineering
```
Email: mohammed.hassan@groupamana.com
Roles: SiteEngineer, DesignEngineer, Viewer, ProjectManager
Department: Engineering
```

### Quality Control
```
Email: layla.ibrahim@groupamana.com
Roles: QCInspector, Viewer
Department: Quality
```

### DuBox Team
```
Email: rania.khalifa@groupamana.com
Roles: DesignEngineer, ProjectManager
Department: DuBox
```

### Procurement
```
Email: noor.alhassan@groupamana.com
Roles: ProcurementOfficer, Viewer
Department: Procurement
```

---

## 📋 Quick Commands

### Start Development Server
```bash
cd dubox-frontend
ng serve
```
**Access:** http://localhost:4200/

### Build for Production
```bash
ng build --configuration production
```

### Run Tests
```bash
ng test
```

---

## 🎯 Usage Examples

### Check User Permission
```typescript
// In your component
constructor(private permissionService: PermissionService) {}

canCreateBox(): boolean {
  return this.permissionService.canCreate('boxes');
}

canApproveQC(): boolean {
  return this.permissionService.hasPermission('qaqc', 'approve');
}
```

### Check User Role
```typescript
// In your component
constructor(private authService: AuthService) {}

isSystemAdmin(): boolean {
  return this.authService.hasRole(UserRole.SystemAdmin);
}

isQCInspector(): boolean {
  return this.authService.hasRole(UserRole.QCInspector);
}
```

### Get User Information
```typescript
const user = this.authService.getCurrentUser();
console.log('All Roles:', user?.allRoles);
console.log('Direct Roles:', user?.directRoles);
console.log('Groups:', user?.groups);
console.log('Department:', user?.department);
```

### Route Protection
```typescript
// In app.routes.ts
{
  path: 'admin',
  canActivate: [authGuard, roleGuard],
  data: { 
    roles: [UserRole.SystemAdmin, UserRole.ProjectManager] 
  },
  loadComponent: () => import('./features/admin/...')
}
```

---

## 🏗️ Project Structure

```
dubox-frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── models/
│   │   │   │   └── user.model.ts          ✅ Updated with 10 roles
│   │   │   ├── services/
│   │   │   │   ├── auth.service.ts        ✅ Updated role handling
│   │   │   │   └── permission.service.ts  ✅ Complete permission matrix
│   │   │   └── guards/
│   │   │       ├── auth.guard.ts
│   │   │       └── role.guard.ts          ✅ Updated for new roles
│   │   ├── shared/
│   │   │   └── components/
│   │   │       ├── header/                ✅ Updated role display
│   │   │       └── sidebar/               ✅ Updated permissions
│   │   └── features/
│   │       ├── auth/
│   │       ├── projects/
│   │       ├── boxes/
│   │       ├── admin/
│   │       └── notifications/
│   └── environments/
│       └── environment.ts                 ✅ Updated with test users
├── GROUP_AMANA_ROLES.md                   ✅ Complete role guide
└── QUICK_REFERENCE.md                     ✅ This file
```

---

## 📡 API Configuration

### Update Backend URL
Edit `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',  // Your backend URL
  apiVersion: 'v1',
};
```

### Expected API Response
```json
{
  "token": "jwt_token_here",
  "refreshToken": "refresh_token_here",
  "user": {
    "id": "user-id",
    "email": "admin@groupamana.com",
    "firstName": "System",
    "lastName": "Administrator",
    "department": "IT",
    "directRoles": ["SystemAdmin"],
    "groups": [
      {
        "id": "group-id",
        "name": "Management",
        "roles": ["SystemAdmin", "ProjectManager"]
      }
    ],
    "allRoles": ["SystemAdmin", "ProjectManager"]
  },
  "expiresIn": 3600
}
```

---

## 🎨 Module Permissions

| Module | View | Create | Edit | Delete | Approve | Export |
|--------|------|--------|------|--------|---------|--------|
| **Projects** | All | PM+ | PM+ | PM+ | PM+ | All |
| **Boxes** | All | PM+ | PM+ | PM+ | QC/Site | Most |
| **QA/QC** | All | QC+ | QC+ | Admin | QC/Site | Most |
| **Users** | Admin+ | Admin | Admin | Admin | - | - |
| **Reports** | All | PM+ | - | - | - | Most |
| **Procurement** | All | PO+ | PO+ | Admin | PO/PM | Most |
| **HSE** | All | HSE+ | HSE+ | Admin | HSE | Most |
| **Costing** | Most | CE+ | CE+ | Admin | CE/PM | Most |

**Legend:**
- All = All roles
- PM+ = ProjectManager and above
- QC = QCInspector
- PO = ProcurementOfficer
- HSE = HSEOfficer
- CE = CostEstimator
- Admin = SystemAdmin only

---

## 🔄 Role Hierarchy (Highest to Lowest)

1. **SystemAdmin** - Full access
2. **ProjectManager** - Project management
3. **DesignEngineer** - Design & modeling
4. **SiteEngineer** - Site oversight
5. **CostEstimator** - Budgeting
6. **QCInspector** - Quality control
7. **Foreman** - Worker supervision
8. **ProcurementOfficer** - Procurement
9. **HSEOfficer** - Safety & environment
10. **Viewer** - Read-only

---

## 🚨 Common Issues & Solutions

### Issue: "Access Denied" or 403 Error
**Solution:** Check user roles match route requirements
```typescript
// Check your route configuration
data: { roles: [UserRole.SystemAdmin, UserRole.ProjectManager] }
```

### Issue: Menu items not showing
**Solution:** Check permission service configuration
```typescript
// Verify permission in sidebar component
permission: { module: 'projects', action: 'view' }
```

### Issue: Role not recognized
**Solution:** Ensure backend returns correct `allRoles` array
```typescript
// Check user object in console
console.log(authService.getCurrentUser());
```

---

## 📚 Documentation Files

- **GROUP_AMANA_ROLES.md** - Complete role & permission guide
- **README.md** - Main project documentation
- **SETUP.md** - Setup instructions
- **TROUBLESHOOTING.md** - Common issues
- **QUICK_REFERENCE.md** - This file

---

## ✅ Testing Checklist

- [ ] Login with SystemAdmin user
- [ ] Verify all menu items visible
- [ ] Login with QCInspector user
- [ ] Verify limited menu items
- [ ] Test permission checks in UI
- [ ] Verify role display in header
- [ ] Test route guards
- [ ] Check unauthorized page access

---

## 🎓 Key Concepts

### Role Inheritance
Users get roles from:
1. **Direct Assignment** (`directRoles`)
2. **Group Membership** (inherited from `groups`)
3. **Combined** in `allRoles`

### Permission Checking
```typescript
// Always use allRoles for permission checks
hasPermission(module, action) {
  user.allRoles.forEach(role => {
    // Check if role has permission
  });
}
```

### Primary Role
The highest permission level role shown in UI:
```typescript
getPrimaryRole(user) {
  // Returns highest priority role from allRoles
  // SystemAdmin > ProjectManager > ... > Viewer
}
```

---

## 🚀 Ready to Test!

1. **Start server:** `cd dubox-frontend && ng serve`
2. **Open browser:** http://localhost:4200/
3. **Login:** Use any test user (password: `AMANA@2024`)
4. **Verify:** Role-based UI and permissions

---

## 📞 Need Help?

- Review **GROUP_AMANA_ROLES.md** for detailed role information
- Check **TROUBLESHOOTING.md** for common issues
- Verify backend API returns correct user structure
- Check browser console for errors

---

© 2024 Group AMANA - DuBox Platform

