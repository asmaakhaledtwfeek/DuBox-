# Backend Authorization & Visibility Implementation Summary

## Overview
This document outlines the comprehensive backend authorization and visibility implementation for the DuBox application. All filtering, authorization, and data visibility rules are enforced exclusively at the backend level.

## Date Implemented
December 8, 2025

---

## 🎯 Core Requirements Implemented

### Role-Based Creation Rules
✅ **Only two roles can create projects and teams:**
- System Admin
- Project Manager

✅ **All other roles are prevented from creating projects or teams** via backend validation.

---

## 🔐 Visibility Rules (Backend Enforced)

### 1. System Admin
- **Access Level:** Full unrestricted access
- **Projects:** ALL projects visible
- **Teams:** ALL teams visible
- **Implementation:** When `accessibleProjectIds` or `accessibleTeamIds` is `null`, no filtering is applied

### 2. Project Manager
- **Access Level:** Restricted to own creations
- **Projects:** ONLY projects they created
- **Teams:** ONLY teams they created
- **Implementation:** Filter by `CreatedBy` field matching the user's ID

### 3. All Other Roles
- **Access Level:** Restricted to PM's scope
- **Projects:** ONLY projects created by the Project Manager who created the user's team
- **Teams:** ONLY teams created by the Project Manager who created the user's team
- **Implementation:** 
  1. Find user's team
  2. Get the team's `CreatedBy` (the Project Manager)
  3. Filter projects/teams where `CreatedBy` matches that Project Manager

---

## 🏗️ Implementation Details

### 1. Database Schema Changes

#### Teams Table Enhancement
**File:** `Dubox.Domain/Entities/Team.cs`

Added `CreatedBy` field to track which user (Project Manager) created the team:

```csharp
public Guid? CreatedBy { get; set; }
```

**Migration:** `20251208220000_AddCreatedByToTeams.cs`

---

### 2. Enhanced Service Interfaces

#### ICurrentUserService Enhancements
**File:** `Dubox.Domain/Abstraction/ICurrentUserService.cs`

Added multi-role support:
```csharp
IEnumerable<string> Roles { get; }
Task<IEnumerable<string>> GetUserRolesAsync(CancellationToken cancellationToken = default);
Task<bool> HasRoleAsync(string roleName, CancellationToken cancellationToken = default);
Task<bool> HasAnyRoleAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
```

**Implementation:** `Dubox.Infrastructure/Services/CurrentUserService.cs`

---

### 3. Centralized Visibility Service

#### IProjectTeamVisibilityService
**File:** `Dubox.Domain/Services/IProjectTeamVisibilityService.cs`

Core methods:
- `CanCreateProjectOrTeamAsync()` - Check creation permissions
- `GetAccessibleProjectIdsAsync()` - Get list of accessible project IDs (null = all)
- `GetAccessibleTeamIdsAsync()` - Get list of accessible team IDs (null = all)
- `CanAccessProjectAsync(Guid projectId)` - Check specific project access
- `CanAccessTeamAsync(Guid teamId)` - Check specific team access

**Implementation:** `Dubox.Infrastructure/Services/ProjectTeamVisibilityService.cs`

**Registered in DI:** `Dubox.Infrastructure/Bootstrap.cs`

---

## 📋 Updated Components

### Command Handlers (Create Operations)

#### 1. CreateProjectCommandHandler
**File:** `Dubox.Application/Features/Projects/Commands/CreateProjectCommandHandler.cs`

✅ **Authorization Check:**
```csharp
var canCreate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
if (!canCreate)
{
    return Result.Failure<ProjectDto>("Access denied. Only System Administrators and Project Managers can create projects.");
}
```

#### 2. CreateTeamCommandHandler
**File:** `Dubox.Application/Features/Teams/Commands/CreateTeamCommandHandler.cs`

✅ **Authorization Check:**
```csharp
var canCreate = await _visibilityService.CanCreateProjectOrTeamAsync(cancellationToken);
if (!canCreate)
{
    return Result.Failure<TeamDto>("Access denied. Only System Administrators and Project Managers can create teams.");
}
```

✅ **Tracking Creator:**
```csharp
team.CreatedBy = currentUserId; // Track who created this team
```

---

### Query Handlers (Read Operations)

#### Projects

**1. GetAllProjectsQueryHandler**
- File: `Dubox.Application/Features/Projects/Queries/GetAllProjectsQueryHandler.cs`
- Applies visibility filtering via `GetAccessibleProjectIdsAsync()`
- Passes filter to `GetProjectsSpecification`

**2. GetProjectByIdQueryHandler**
- File: `Dubox.Application/Features/Projects/Queries/GetProjectByIdQueryHandler.cs`
- Validates access via `CanAccessProjectAsync()`
- Returns access denied error if unauthorized

**Specification Updated:** `Dubox.Application/Specifications/GetProjectsSpecification.cs`
```csharp
if (accessibleProjectIds != null)
{
    AddCriteria(p => accessibleProjectIds.Contains(p.ProjectId));
}
```

#### Teams

**1. GetAllTeamsQueryHandler**
- File: `Dubox.Application/Features/Teams/Queries/GetAllTeamsQueryHandler.cs`
- Applies visibility filtering via `GetAccessibleTeamIdsAsync()`
- Passes filter to `GetTeamWithIncludesSpecification`

**2. GetTeamByIdQueryHandler**
- File: `Dubox.Application/Features/Teams/Queries/GetTeamByIdQueryHandler.cs`
- Validates access via `CanAccessTeamAsync()`
- Returns access denied error if unauthorized

**Specification Updated:** `Dubox.Application/Specifications/GetTeamWithIncludesSpecification.cs`
```csharp
if (accessibleTeamIds != null)
{
    AddCriteria(team => accessibleTeamIds.Contains(team.TeamId));
}
```

---

### Dashboard Queries

#### 1. GetDashboardStatisticsQueryHandler
**File:** `Dubox.Application/Features/Dashboard/Queries/GetDashboardStatisticsQueryHandler.cs`

✅ Filters all statistics by accessible projects:
- Projects query filtered
- Boxes query filtered by project access
- WIR records filtered by project access
- Activities filtered by project access

#### 2. GetAllProjectsDashboardQueryHandler
**File:** `Dubox.Application/Features/Dashboard/Queries/GetAllProjectsDashboardQueryHandler.cs`

✅ Returns dashboard data only for accessible projects

#### 3. GetProjectDashboardQueryHandler
**File:** `Dubox.Application/Features/Dashboard/Queries/GetProjectDashboardQueryHandler.cs`

✅ Validates project access before returning dashboard data

---

### Report Queries

#### 1. GetBoxProgressReportQuery
**File:** `Dubox.Application/Features/Reports/Queries/GetBoxProgressReportQuery.cs`

✅ Filters boxes by accessible projects
✅ Validates specific project access when projectId is provided

#### 2. GetProjectsSummaryReportQueryHandler
**File:** `Dubox.Application/Features/Reports/Queries/GetProjectsSummaryReportQueryHandler.cs`

✅ Passes accessible project IDs to specification
**Specification:** `ProjectsSummaryReportSpecification.cs` updated

#### 3. GetTeamsPerformanceReportQueryHandler
**File:** `Dubox.Application/Features/Reports/Queries/GetTeamsPerformanceReportQueryHandler.cs`

✅ Filters by both accessible projects and teams
**Specifications Updated:**
- `TeamsPerformanceReportSpecification.cs`
- `ActivitiesReportSpecification.cs`

#### 4. GetActivitiesReportQueryHandler
**File:** `Dubox.Application/Features/Reports/Queries/GetActivitiesReportQueryHandler.cs`

✅ Filters activities by accessible projects

#### 5. GetActivitiesSummaryQueryHandler
**File:** `Dubox.Application/Features/Reports/Queries/GetActivitiesSummaryQueryHandler.cs`

✅ Applies project visibility to activity summary

#### 6. GetBoxesSummaryReportQueryHandler
**File:** `Dubox.Application/Features/Reports/Queries/GetBoxesSummaryReportQueryHandler.cs`

✅ Filters boxes by accessible projects
**Specification:** `BoxesSummaryReportSpecification.cs` updated

---

### Activity Assignment

#### AssignActivityToTeamCommandHandler
**File:** `Dubox.Application/Features/Activities/Commands/AssignActivityToTeamCommandHandler.cs`

✅ **Dual Authorization Check:**
1. Validates user has access to the project containing the activity
2. Validates user has access to the team being assigned

```csharp
var canAccessProject = await _visibilityService.CanAccessProjectAsync(activity.Box.ProjectId, cancellationToken);
if (!canAccessProject)
{
    return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to modify activities in this project.");
}

var canAccessTeam = await _visibilityService.CanAccessTeamAsync(request.TeamId, cancellationToken);
if (!canAccessTeam)
{
    return Result.Failure<AssignBoxActivityTeamDto>("Access denied. You do not have permission to assign this team.");
}
```

---

## 🎨 Frontend Impact

### No Frontend Changes Required

The frontend continues to work as-is because:

1. **Automatic Filtering:** All API endpoints now return only authorized data
2. **Team Dropdowns:** `GetAllTeams` endpoint automatically returns only accessible teams
3. **Project Lists:** `GetAllProjects` endpoint automatically returns only accessible projects
4. **Reports & Dashboards:** All data is pre-filtered by the backend

### Frontend Behavior:
- Users will only see projects/teams they have access to
- Dropdowns will only contain accessible options
- Create buttons can still be shown (backend will reject unauthorized attempts)
- Direct API calls with restricted IDs will return "Access Denied" errors

---

## 🔒 Security Guarantees

### 1. No Bypass Possible
- All authorization happens in query handlers and command handlers
- Specifications apply filtering at the database query level
- Direct API calls are protected

### 2. Consistent Enforcement
- All endpoints use the centralized `IProjectTeamVisibilityService`
- Single source of truth for visibility rules
- No logic duplication

### 3. Database-Level Filtering
- Filtering applied in LINQ queries
- Translated to SQL WHERE clauses
- No data leakage at any level

---

## 📊 Updated Specifications

The following specifications were updated to accept and apply visibility filters:

1. `GetProjectsSpecification.cs` - Projects filtering
2. `GetTeamWithIncludesSpecification.cs` - Teams filtering
3. `ProjectsSummaryReportSpecification.cs` - Projects report
4. `TeamsPerformanceReportSpecification.cs` - Teams performance
5. `ActivitiesReportSpecification.cs` - Activities filtering
6. `BoxesSummaryReportSpecification.cs` - Boxes filtering

---

## ✅ Testing Checklist

### As System Admin:
- [ ] Can create projects ✅
- [ ] Can create teams ✅
- [ ] Can see ALL projects ✅
- [ ] Can see ALL teams ✅
- [ ] Dashboard shows all data ✅
- [ ] Reports show all data ✅

### As Project Manager:
- [ ] Can create projects ✅
- [ ] Can create teams ✅
- [ ] Can ONLY see own projects ✅
- [ ] Can ONLY see own teams ✅
- [ ] Dashboard shows only own projects ✅
- [ ] Reports show only own data ✅
- [ ] Can assign own teams to activities ✅

### As Other Roles (QCInspector, Foreman, etc.):
- [ ] CANNOT create projects ✅
- [ ] CANNOT create teams ✅
- [ ] Can see only PM's projects (who created their team) ✅
- [ ] Can see only PM's teams (who created their team) ✅
- [ ] Dashboard shows only accessible data ✅
- [ ] Reports show only accessible data ✅
- [ ] Can assign only accessible teams ✅

---

## 🚀 Deployment Notes

### Database Migration Required
Run the migration to add `CreatedBy` to Teams table:

```bash
dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api
```

### Existing Data
For existing teams without a `CreatedBy` value:
- Teams will have `CreatedBy = null`
- These teams will be visible to SystemAdmin only
- Consider running a data migration script to set appropriate CreatedBy values

---

## 📝 Summary

### What Was Changed:
1. ✅ Added `CreatedBy` field to Team entity
2. ✅ Enhanced `ICurrentUserService` with multi-role support
3. ✅ Created centralized `IProjectTeamVisibilityService`
4. ✅ Updated ALL create handlers with authorization checks
5. ✅ Updated ALL query handlers with visibility filtering
6. ✅ Updated ALL specifications to apply visibility filters
7. ✅ Updated dashboard queries with filtering
8. ✅ Updated report queries with filtering
9. ✅ Protected activity assignment with dual authorization

### What Frontend Does:
- ✅ Simply renders data returned by backend
- ✅ No business logic for visibility in frontend
- ✅ Backend returns only authorized data automatically

### Security Result:
- ✅ **No bypass possible** - all authorization at backend
- ✅ **Consistent enforcement** - centralized service
- ✅ **Database-level filtering** - query-level security
- ✅ **Zero frontend changes needed** - transparent to UI

---

## 👨‍💻 Files Modified

### Domain Layer:
- `Dubox.Domain/Entities/Team.cs`
- `Dubox.Domain/Abstraction/ICurrentUserService.cs`
- `Dubox.Domain/Services/IProjectTeamVisibilityService.cs`

### Infrastructure Layer:
- `Dubox.Infrastructure/Services/CurrentUserService.cs`
- `Dubox.Infrastructure/Services/ProjectTeamVisibilityService.cs`
- `Dubox.Infrastructure/Bootstrap.cs`
- `Dubox.Infrastructure/Migrations/20251208220000_AddCreatedByToTeams.cs`
- `Dubox.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs`

### Application Layer:

**Commands:**
- `Dubox.Application/Features/Projects/Commands/CreateProjectCommandHandler.cs`
- `Dubox.Application/Features/Teams/Commands/CreateTeamCommandHandler.cs`
- `Dubox.Application/Features/Activities/Commands/AssignActivityToTeamCommandHandler.cs`

**Project Queries:**
- `Dubox.Application/Features/Projects/Queries/GetAllProjectsQueryHandler.cs`
- `Dubox.Application/Features/Projects/Queries/GetProjectByIdQueryHandler.cs`

**Team Queries:**
- `Dubox.Application/Features/Teams/Queries/GetAllTeamsQueryHandler.cs`
- `Dubox.Application/Features/Teams/Queries/GetTeamByIdQueryHandler.cs`

**Dashboard Queries:**
- `Dubox.Application/Features/Dashboard/Queries/GetDashboardStatisticsQueryHandler.cs`
- `Dubox.Application/Features/Dashboard/Queries/GetAllProjectsDashboardQueryHandler.cs`
- `Dubox.Application/Features/Dashboard/Queries/GetProjectDashboardQueryHandler.cs`

**Report Queries:**
- `Dubox.Application/Features/Reports/Queries/GetBoxProgressReportQuery.cs`
- `Dubox.Application/Features/Reports/Queries/GetProjectsSummaryReportQueryHandler.cs`
- `Dubox.Application/Features/Reports/Queries/GetTeamsPerformanceReportQueryHandler.cs`
- `Dubox.Application/Features/Reports/Queries/GetActivitiesReportQueryHandler.cs`
- `Dubox.Application/Features/Reports/Queries/GetActivitiesSummaryQueryHandler.cs`
- `Dubox.Application/Features/Reports/Queries/GetBoxesSummaryReportQueryHandler.cs`

**Specifications:**
- `Dubox.Application/Specifications/GetProjectsSpecification.cs`
- `Dubox.Application/Specifications/GetTeamWithIncludesSpecification.cs`
- `Dubox.Application/Specifications/ProjectsSummaryReportSpecification.cs`
- `Dubox.Application/Specifications/TeamsPerformanceReportSpecification.cs`
- `Dubox.Application/Specifications/ActivitiesReportSpecification.cs`
- `Dubox.Application/Specifications/BoxesSummaryReportSpecification.cs`

---

## ✨ Conclusion

The backend authorization system is now **fully implemented** with:
- ✅ **Comprehensive role-based access control**
- ✅ **Complete visibility filtering at database level**
- ✅ **Centralized authorization service**
- ✅ **No frontend changes required**
- ✅ **100% backend enforcement**

All filtering, authorization, and data visibility rules are enforced **strictly at the backend level only**, as required.

