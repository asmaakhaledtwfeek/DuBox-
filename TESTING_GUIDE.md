# DuBox Platform - Testing Guide

## 🎯 Overview

This guide helps you test the Role-Based Access Control (RBAC) system after the backend changes.

---

## 🚀 Quick Start

### 1. Build and Run Backend

```powershell
# Navigate to API project
cd Dubox.Api

# Build the solution
dotnet build

# Run the API
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7098
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5098
```

---

### 2. Run Frontend

```powershell
# Navigate to frontend folder
cd dubox-frontend

# Run Angular development server
ng serve
```

**Expected Output:**
```
✔ Browser application bundle generation complete.
** Angular Live Development Server is listening on localhost:4200 **
```

---

## 🧪 Test Scenarios

### Test 1: System Admin Login

**User:** admin@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Redirected to Projects page
3. ✅ Sidebar shows all menu items:
   - Projects
   - Boxes
   - Admin Panel
   - User Management
   - Notifications
   - QA/QC Checklist
   - Procurement
   - HSE
   - Costing
   - Reports
4. ✅ Header shows:
   - Name: "System Administrator"
   - Role: "System Admin"
   - Department: "IT"

**Check Console:**
```
✅ Roles loaded from backend for admin@groupamana.com
Direct Roles: ["SystemAdmin"]
All Roles: ["SystemAdmin", "ProjectManager"]
Groups: Management
```

---

### Test 2: Site Engineer with Group Roles

**User:** mohammed.hassan@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Has roles from Engineering group + direct ProjectManager role
3. ✅ Sidebar shows:
   - Projects ✅
   - Boxes ✅
   - Admin Panel ✅ (because has ProjectManager role)
   - User Management ✅ (because has ProjectManager role)
   - Notifications ✅
   - QA/QC Checklist ❌ (not visible - needs QCInspector)
   - Procurement ❌ (not visible)
   - HSE ❌ (not visible)
   - Costing ❌ (not visible)
   - Reports ✅ (ProjectManager can view)
4. ✅ Header shows:
   - Name: "Mohammed Hassan"
   - Primary Role: "Project Manager" (highest priority)
   - Department: "Engineering"

**Check Console:**
```
✅ Roles loaded from backend for mohammed.hassan@groupamana.com
Direct Roles: ["ProjectManager"]
All Roles: ["ProjectManager", "SiteEngineer", "DesignEngineer", "Viewer"]
Groups: Engineering
```

---

### Test 3: QC Inspector

**User:** layla.ibrahim@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Has QCInspector and Viewer roles from QualityControl group
3. ✅ Sidebar shows:
   - Projects ✅
   - Boxes ✅
   - Admin Panel ❌ (not visible)
   - User Management ❌ (not visible)
   - Notifications ✅
   - QA/QC Checklist ✅ (QCInspector can view)
   - Procurement ❌
   - HSE ❌
   - Costing ❌
   - Reports ✅ (Viewer can view)
4. ✅ Header shows:
   - Name: "Layla Ibrahim"
   - Primary Role: "QC Inspector"
   - Department: "Quality"

**Check Console:**
```
✅ Roles loaded from backend for layla.ibrahim@groupamana.com
Direct Roles: []
All Roles: ["QCInspector", "Viewer"]
Groups: QualityControl
```

---

### Test 4: Procurement Officer

**User:** noor.alhassan@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Has ProcurementOfficer and Viewer roles
3. ✅ Sidebar shows:
   - Projects ✅
   - Boxes ❌
   - Admin Panel ❌
   - User Management ❌
   - Notifications ✅
   - QA/QC Checklist ❌
   - Procurement ✅ (ProcurementOfficer can view)
   - HSE ❌
   - Costing ❌
   - Reports ✅ (Viewer can view)

---

### Test 5: HSE Officer

**User:** maryam.said@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Has HSEOfficer and Viewer roles
3. ✅ Sidebar shows:
   - Projects ✅
   - Boxes ❌
   - Admin Panel ❌
   - User Management ❌
   - Notifications ✅
   - QA/QC Checklist ❌
   - Procurement ❌
   - HSE ✅ (HSEOfficer can view)
   - Costing ❌
   - Reports ✅ (Viewer can view)

---

### Test 6: DuBox Team Member

**User:** rania.khalifa@groupamana.com  
**Password:** AMANA@2024

**Expected Behavior:**
1. ✅ Login successful
2. ✅ Has DesignEngineer and ProjectManager roles from DuBoxTeam group
3. ✅ Sidebar shows:
   - Projects ✅
   - Boxes ✅ (DesignEngineer can view)
   - Admin Panel ✅ (ProjectManager can view)
   - User Management ✅ (ProjectManager can view)
   - Notifications ✅
   - QA/QC Checklist ❌
   - Procurement ❌
   - HSE ❌
   - Costing ❌
   - Reports ✅

---

## 🔍 API Testing with Postman/Swagger

### Login Endpoint

**Endpoint:** `POST https://localhost:7098/api/auth/login`

**Request Body:**
```json
{
  "email": "admin@groupamana.com",
  "password": "AMANA@2024"
}
```

**Expected Response:**
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "user": {
      "userId": "f1111111-1111-1111-1111-111111111111",
      "email": "admin@groupamana.com",
      "fullName": "System Administrator",
      "departmentId": "d1000000-0000-0000-0000-000000000001",
      "department": "IT",
      "isActive": true,
      "lastLoginDate": "2025-11-16T10:30:00Z",
      "createdDate": "2024-11-01T00:00:00Z",
      "directRoles": ["SystemAdmin"],
      "groups": [
        {
          "groupId": "g1000000-0000-0000-0000-000000000001",
          "groupName": "Management",
          "roles": [
            {
              "roleId": "r1000000-0000-0000-0000-000000000001",
              "roleName": "SystemAdmin",
              "description": "Full system administration access",
              "isActive": true,
              "createdDate": "2024-11-01T00:00:00Z"
            },
            {
              "roleId": "r2000000-0000-0000-0000-000000000001",
              "roleName": "ProjectManager",
              "description": "Manage projects and teams",
              "isActive": true,
              "createdDate": "2024-11-01T00:00:00Z"
            }
          ]
        }
      ],
      "allRoles": ["SystemAdmin", "ProjectManager"]
    }
  },
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

---

## ✅ Validation Checklist

### Backend Validation

- [ ] Backend compiles without errors
- [ ] Login endpoint returns `allRoles`, `directRoles`, and `groups`
- [ ] `allRoles` contains distinct combined roles (direct + group)
- [ ] `groups` includes all group memberships with their roles
- [ ] `department` name is included (not just ID)
- [ ] Token is generated successfully
- [ ] Response structure matches expected format

### Frontend Validation

- [ ] Frontend compiles without errors
- [ ] Login works with real backend
- [ ] Roles are loaded from backend (check console)
- [ ] Temporary workaround is no longer triggered
- [ ] Sidebar menu items show/hide based on permissions
- [ ] Header displays correct user name, role, and department
- [ ] Role priority is correct (SystemAdmin shows before ProjectManager)
- [ ] Navigation guards work (unauthorized routes redirect)
- [ ] Permission checks work throughout the application

---

## 🐛 Troubleshooting

### Issue: Backend returns 500 error on login

**Check:**
1. Database is running
2. Connection string is correct
3. Migrations have been applied
4. Seed data has been run
5. Entity relationships are properly configured

**Solution:**
```powershell
# Apply migrations
dotnet ef database update

# Re-run seed data (if exists)
# Check Dubox.Api or Dubox.Infrastructure for seed scripts
```

---

### Issue: Roles are empty in frontend

**Check Browser Console:**
```
⚠️ Backend did not return roles. Using default Viewer role.
```

**Solution:**
1. Verify backend is returning roles in response
2. Check database: User has roles assigned
3. Check database: User has groups assigned
4. Check database: Groups have roles assigned

**Database Queries:**
```sql
-- Check user roles
SELECT u.Email, r.RoleName
FROM Users u
JOIN UserRoles ur ON u.UserId = ur.UserId
JOIN Roles r ON ur.RoleId = r.RoleId
WHERE u.Email = 'admin@groupamana.com';

-- Check user groups
SELECT u.Email, g.GroupName
FROM Users u
JOIN UserGroups ug ON u.UserId = ug.UserId
JOIN Groups g ON ug.GroupId = g.GroupId
WHERE u.Email = 'admin@groupamana.com';

-- Check group roles
SELECT g.GroupName, r.RoleName
FROM Groups g
JOIN GroupRoles gr ON g.GroupId = gr.GroupId
JOIN Roles r ON gr.RoleId = r.RoleId;
```

---

### Issue: Sidebar items not showing

**Check:**
1. User has required role
2. `PermissionService` is checking permissions correctly
3. Console shows loaded roles
4. `hasAccess()` method in sidebar component

**Debug:**
```typescript
// In sidebar.component.ts, add debug logging
hasAccess(item: MenuItem): boolean {
  console.log('Checking access for:', item.label, item.requiredPermissions);
  const hasAccess = item.requiredPermissions?.every(perm => 
    this.permissionService.hasPermission(perm.module, perm.action)
  ) ?? true;
  console.log('Access result:', hasAccess);
  return hasAccess;
}
```

---

### Issue: CORS error when calling backend

**Solution:**
Update `Program.cs` in Dubox.Api:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// After app.Build()
app.UseCors("AllowAngular");
```

---

## 📊 Expected Test Results Summary

| User | Direct Roles | Group Roles | Sidebar Items Visible |
|------|-------------|-------------|----------------------|
| admin@groupamana.com | SystemAdmin | ProjectManager (via Management) | All (10/10) |
| mohammed.hassan@groupamana.com | ProjectManager | SiteEngineer, DesignEngineer, Viewer (via Engineering) | 7/10 |
| layla.ibrahim@groupamana.com | - | QCInspector, Viewer (via QualityControl) | 5/10 |
| noor.alhassan@groupamana.com | - | ProcurementOfficer, Viewer (via Procurement) | 4/10 |
| maryam.said@groupamana.com | - | HSEOfficer, Viewer (via HSE) | 4/10 |
| rania.khalifa@groupamana.com | - | DesignEngineer, ProjectManager (via DuBoxTeam) | 7/10 |

---

## 🎉 Success Criteria

✅ **Backend:**
- All users can login successfully
- Response includes roles, groups, and department
- No null or empty role arrays (unless user has no roles)

✅ **Frontend:**
- Console shows "✅ Roles loaded from backend"
- No temporary workaround triggered
- Sidebar dynamically shows/hides based on permissions
- Header displays correct user information
- Navigation guards protect routes correctly

✅ **Integration:**
- Frontend correctly parses backend response
- Permissions work as expected
- Role priority is correct
- Group role inheritance works

---

## 📞 Support

If you encounter issues:

1. Check console for error messages
2. Verify database has seed data
3. Check API response in Network tab
4. Review BACKEND_CHANGES.md
5. Check RBAC configuration files

---

© 2024 Group AMANA - DuBox Platform

