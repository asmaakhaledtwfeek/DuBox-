# Project Manager Visibility Fix

## Issue Description
When a Project Manager creates a project, it was being saved to the database successfully, but the Project Manager couldn't see it in their dashboard. However, System Admins could see all projects including those created by Project Managers.

## Root Cause
The `CreateProjectCommandHandler` was not setting the `CreatedBy` field when creating a new project. This field is critical because the visibility filtering logic for Project Managers specifically looks for projects where `CreatedBy` matches their user ID.

**Visibility Logic (ProjectTeamVisibilityService):**
```csharp
// For Project Manager: Return only projects created by this user
var projectIds = await _context.Projects
    .Where(p => p.CreatedBy == userId.ToString())
    .Select(p => p.ProjectId)
    .ToListAsync(cancellationToken);
```

Without `CreatedBy` being set, the project would have a null value, and the filter would never match, making the project invisible to the Project Manager who created it.

## Solution Applied

### 1. Fixed CreateProjectCommandHandler
**File:** `Dubox.Application/Features/Projects/Commands/CreateProjectCommandHandler.cs`

**Added:**
```csharp
// Set CreatedBy to track who created this project (critical for Project Manager visibility)
project.CreatedBy = currentUserId.ToString();
```

This ensures that when a Project Manager creates a project, their user ID is properly stored in the `CreatedBy` field.

### 2. Enhanced UpdateProjectCommandHandler
**File:** `Dubox.Application/Features/Projects/Commands/UpdateProjectCommandHandler.cs`

**Added:**
```csharp
project.ModifiedBy = currentUserId.ToString();
```

This ensures proper audit tracking of who modifies projects.

## Testing Instructions

### Prerequisites
- Have a Project Manager user account ready
- Have a System Admin user account ready

### Test Scenario 1: Create New Project as Project Manager

1. **Login as Project Manager**
   - Email: `mohammed.hassan@groupamana.com` (or any PM user)
   - Password: `AMANA@2024`

2. **Create a New Project**
   - Click "+ New Project" button
   - Fill in project details:
     - Project Code: `TEST-PM-001`
     - Project Name: `Test PM Visibility`
     - Client Name: `Test Client`
     - Location: `Test Location`
     - Planned Start Date: (any future date)
     - Duration: `90` days
   - Click "Create"

3. **Verify Project Appears**
   - ✅ The new project should appear in the Project Manager's project list immediately
   - ✅ The project should be visible in the Projects page
   - ✅ The project should appear in the Dashboard

4. **Verify Project Details**
   - Open the project details
   - Verify all information is correct
   - Verify you can update the project

### Test Scenario 2: Verify Other PM Cannot See It

1. **Login as Different Project Manager**
   - Email: `rania.khalifa@groupamana.com` (different PM)
   - Password: `AMANA@2024`

2. **Check Projects List**
   - ✅ The project created by the first PM should NOT be visible
   - ✅ Only projects created by this PM should be visible

### Test Scenario 3: Verify System Admin Can See All

1. **Login as System Admin**
   - Email: `admin@groupamana.com`
   - Password: `AMANA@2024`

2. **Check Projects List**
   - ✅ All projects should be visible, including those created by both PMs
   - ✅ System Admin should see the full project list

### Test Scenario 4: Update Project

1. **Login as Project Manager (original creator)**
   - Login with the PM who created the test project

2. **Update the Project**
   - Navigate to the project
   - Click "Edit" or update any field
   - Save changes

3. **Verify Updates**
   - ✅ Changes should save successfully
   - ✅ Project should still be visible to the PM
   - ✅ `ModifiedBy` field should be set in the database

## Database Verification

To verify the fix is working, check the database:

```sql
-- Check that CreatedBy is set for new projects
SELECT ProjectId, ProjectCode, ProjectName, CreatedBy, CreatedDate
FROM Projects
WHERE ProjectCode = 'TEST-PM-001';

-- Expected Result:
-- CreatedBy should contain the Project Manager's User ID (GUID as string)
```

## For Existing Projects

**Important:** Projects created BEFORE this fix will have `CreatedBy = NULL`. These projects will be:
- ✅ Visible to System Admin (sees everything)
- ✅ Visible to Viewer (sees everything)
- ❌ NOT visible to any Project Manager (no CreatedBy match)

### Option 1: Update Existing Projects Manually
If you have existing projects that need to be assigned to specific Project Managers:

```sql
-- Update existing projects to assign them to a specific Project Manager
UPDATE Projects
SET CreatedBy = '<ProjectManager-UserId-Guid>'
WHERE CreatedBy IS NULL
  AND ProjectCode IN ('PROJECT-001', 'PROJECT-002'); -- Specify which projects
```

### Option 2: Assign All Existing Projects to System Admin
This makes them visible to everyone:

```sql
-- Assign all orphaned projects to System Admin (they'll see them anyway)
UPDATE Projects
SET CreatedBy = (SELECT UserId FROM Users WHERE Email = 'admin@groupamana.com')
WHERE CreatedBy IS NULL;
```

## Expected Behavior After Fix

### System Admin
- ✅ Sees ALL projects (including those with NULL CreatedBy)
- ✅ Can create, update, delete any project

### Viewer
- ✅ Sees ALL projects (read-only)
- ❌ Cannot create or modify projects

### Project Manager
- ✅ Sees ONLY projects they created (where CreatedBy = their UserId)
- ✅ Can create new projects (will automatically be visible to them)
- ✅ Can update their own projects
- ❌ Cannot see projects created by other Project Managers
- ❌ Cannot see projects with NULL CreatedBy

### Other Roles (QC Inspector, Foreman, etc.)
- ✅ See only projects created by the PM who created their team
- ❌ Cannot create projects
- Can work within their assigned projects

## Files Modified

1. `Dubox.Application/Features/Projects/Commands/CreateProjectCommandHandler.cs`
   - Added: `project.CreatedBy = currentUserId.ToString();`

2. `Dubox.Application/Features/Projects/Commands/UpdateProjectCommandHandler.cs`
   - Added: `project.ModifiedBy = currentUserId.ToString();`

## Summary

This fix ensures that:
1. ✅ Project Managers can see projects they create immediately
2. ✅ Proper audit tracking is maintained
3. ✅ Role-based visibility works as designed
4. ✅ No frontend changes are required
5. ✅ The fix is backward compatible (old projects with NULL CreatedBy can be fixed with SQL update)

## Deployment Notes

- ✅ No database migration required
- ✅ No schema changes
- ✅ Backend-only code change
- ✅ Can be deployed immediately
- ⚠️ Consider updating existing projects with NULL CreatedBy (see options above)

---

**Date Fixed:** December 9, 2025  
**Severity:** High (Broken PM visibility)  
**Impact:** Backend only  
**Testing Required:** Yes (follow test scenarios above)

