# Group AMANA - DuBox Role & Permission Guide

## 🔐 Authentication

**Default Password for All Users:** `AMANA@2024`

**Login Endpoint:** `POST /api/auth/login`

```json
{
  "email": "admin@groupamana.com",
  "password": "AMANA@2024"
}
```

---

## 👥 User Roles (10 Roles)

### 1. SystemAdmin
**Full system administration access**
- All modules: view, create, edit, delete, manage
- User management and role assignment
- System configuration

**Test Users:**
- admin@groupamana.com
- ahmed.almazrouei@groupamana.com
- sara.alkhan@groupamana.com

---

### 2. ProjectManager
**Manage projects and teams**
- Projects: view, create, edit, delete, manage, export
- Boxes: view, create, edit, delete, update-status, manage
- Activities: view, create, edit, approve, reject
- QA/QC: view, approve, reject
- Reports: view, create, export

**Test Users:**
- All Management group members
- mohammed.hassan@groupamana.com (also has Engineering roles)
- rania.khalifa@groupamana.com (DuBox Team)
- salim.rashid@groupamana.com (DuBox Team)

---

### 3. DesignEngineer
**Design and BIM modeling**
- Projects: view, create, edit, export
- Boxes: view, create, edit, update-status
- Activities: view, create, edit, submit
- QA/QC: view, submit

**Test Users:**
- mohammed.hassan@groupamana.com
- fatima.alali@groupamana.com
- khalid.omar@groupamana.com
- rania.khalifa@groupamana.com (DuBox)
- huda.almarri@groupamana.com (DuPod)

---

### 4. SiteEngineer
**Oversee construction site activities**
- Projects: view, edit, export
- Boxes: view, edit, update-status
- Activities: view, create, edit, approve, reject
- QA/QC: view, approve, reject

**Test Users:**
- mohammed.hassan@groupamana.com
- fatima.alali@groupamana.com
- khalid.omar@groupamana.com

---

### 5. QCInspector
**Quality control and inspection**
- Projects: view
- Boxes: view, update-status
- Activities: view, approve, reject
- QA/QC: view, create, edit, approve, reject (PRIMARY ROLE)

**Test Users:**
- layla.ibrahim@groupamana.com
- hamza.khalil@groupamana.com

---

### 6. Foreman
**Supervise construction workers**
- Projects: view
- Boxes: view, update-status
- Activities: view, edit, submit
- QA/QC: view, submit

**Test Users:**
- ali.mohammed@groupamana.com
- omar.saleh@groupamana.com
- youssef.ahmed@groupamana.com

---

### 7. ProcurementOfficer
**Handle material procurement**
- Projects: view
- Boxes: view
- Procurement: view, create, edit, approve (PRIMARY)
- Reports: view, export

**Test Users:**
- noor.alhassan@groupamana.com
- zaid.mansour@groupamana.com

---

### 8. HSEOfficer
**Health, Safety, and Environment oversight**
- Projects: view
- Boxes: view
- HSE: view, create, edit, approve (PRIMARY)
- QA/QC: view, approve
- Reports: view, export

**Test Users:**
- maryam.Said@groupamana.com
- tariq.abdullah@groupamana.com

---

### 9. CostEstimator
**Cost estimation and budgeting**
- Projects: view, export
- Boxes: view, export
- Costing: view, create, edit, approve (PRIMARY)
- Reports: view, create, export

**Test Users:**
- ahmed.almazrouei@groupamana.com (also has Admin roles)

---

### 10. Viewer
**Read-only access to projects**
- All modules: view only
- No edit, create, or approval permissions

**Note:** Viewer role is inherited by all groups as a baseline permission

---

## 🏢 Groups & Role Inheritance

### Management Group
**Roles:** SystemAdmin, ProjectManager
- admin@groupamana.com
- ahmed.almazrouei@groupamana.com
- sara.alkhan@groupamana.com

### Engineering Group
**Roles:** SiteEngineer, DesignEngineer, Viewer
- mohammed.hassan@groupamana.com
- fatima.alali@groupamana.com
- khalid.omar@groupamana.com

### Construction Group
**Roles:** Foreman, Viewer
- ali.mohammed@groupamana.com
- omar.saleh@groupamana.com
- youssef.ahmed@groupamana.com

### QualityControl Group
**Roles:** QCInspector, Viewer
- layla.ibrahim@groupamana.com
- hamza.khalil@groupamana.com

### Procurement Group
**Roles:** ProcurementOfficer, Viewer
- noor.alhassan@groupamana.com
- zaid.mansour@groupamana.com

### HSE Group
**Roles:** HSEOfficer, Viewer
- maryam.Said@groupamana.com
- tariq.abdullah@groupamana.com

### DuBoxTeam Group (Modular Construction)
**Roles:** DesignEngineer, ProjectManager
- rania.khalifa@groupamana.com
- salim.rashid@groupamana.com

### DuPodTeam Group (Plug-and-Play Modular)
**Roles:** DesignEngineer, ProjectManager
- huda.almarri@groupamana.com
- faisal.sultan@groupamana.com

---

## 📋 Module Permissions Matrix

| Module | SystemAdmin | ProjectManager | DesignEngineer | SiteEngineer | QCInspector | Foreman | Others |
|--------|-------------|----------------|----------------|--------------|-------------|---------|--------|
| **Projects** | Full | Full | View, Create, Edit | View, Edit | View | View | View |
| **Boxes** | Full | Full | View, Create, Edit | View, Edit | View, Update | View, Update | View |
| **Activities** | Full | Full | View, Create, Submit | View, Approve | View, Approve | View, Submit | View |
| **QA/QC** | Full | Full | View, Submit | View, Approve | Full QC | View, Submit | View |
| **Users** | Full | View, Edit | - | - | - | - | - |
| **Reports** | Full | Full | View, Export | View, Export | View, Export | View | View |
| **Procurement** | Full | View, Approve | - | - | - | - | Full (ProcurementOfficer) |
| **HSE** | Full | View, Create | View | View, Create | View | View, Create | Full (HSEOfficer) |
| **Costing** | Full | Full | View, Create | View | - | - | Full (CostEstimator) |

---

## 🔒 Permission Actions

### Standard Actions
- **view** - Read-only access
- **create** - Create new records
- **edit** - Modify existing records
- **delete** - Remove records
- **export** - Export data/reports
- **manage** - Full administrative control

### Workflow Actions
- **approve** - Approve activities/submissions
- **reject** - Reject and send back for revision
- **submit** - Submit for approval
- **update-status** - Change status of items

---

## 💡 Usage Examples

### Example 1: Check if user can create boxes
```typescript
import { PermissionService } from '@core/services/permission.service';

constructor(private permissionService: PermissionService) {}

canCreateBox(): boolean {
  return this.permissionService.canCreate('boxes');
}
```

### Example 2: Check specific role
```typescript
import { AuthService } from '@core/services/auth.service';
import { UserRole } from '@core/models/user.model';

constructor(private authService: AuthService) {}

isQCInspector(): boolean {
  return this.authService.hasRole(UserRole.QCInspector);
}
```

### Example 3: Check multiple permissions
```typescript
canApproveQAQC(): boolean {
  return this.permissionService.hasPermission('qaqc', 'approve');
}
```

### Example 4: Get user's all roles
```typescript
getUserRoles(): string[] {
  const user = this.authService.getCurrentUser();
  return user?.allRoles || [];
}
```

---

## 🧪 Testing with Different Roles

### Test SystemAdmin Access
```bash
Email: admin@groupamana.com
Password: AMANA@2024
Expected: Full access to all modules
```

### Test QC Inspector Access
```bash
Email: layla.ibrahim@groupamana.com
Password: AMANA@2024
Expected: QA/QC approval access, view-only for projects
```

### Test Foreman Access
```bash
Email: ali.mohammed@groupamana.com
Password: AMANA@2024
Expected: Box status updates, activity submission, view projects
```

### Test DuBox Team Access
```bash
Email: rania.khalifa@groupamana.com
Password: AMANA@2024
Expected: Design + Project Management capabilities
```

---

## 🔄 Role Inheritance

Users receive roles from two sources:

1. **Direct Roles** - Assigned directly to the user
2. **Group Roles** - Inherited from group membership

**Example: mohammed.hassan@groupamana.com**
- Direct Roles: ProjectManager
- Group: Engineering (SiteEngineer, DesignEngineer, Viewer)
- **All Roles:** ProjectManager, SiteEngineer, DesignEngineer, Viewer

The frontend checks `user.allRoles` which includes both direct and inherited roles.

---

## 📝 API Response Format

### Login Response
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "refresh_token_here",
  "user": {
    "id": "user-id",
    "email": "admin@groupamana.com",
    "firstName": "System",
    "lastName": "Administrator",
    "department": "IT",
    "directRoles": ["SystemAdmin", "ProjectManager"],
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

## 🚀 Quick Start

1. **Update API URL** in `src/environments/environment.ts`
2. **Start the app**: `ng serve`
3. **Login** with any test user
4. **Verify permissions** based on user role

---

© 2024 Group AMANA - DuBox Platform

