# Quality Issues Visibility Update

## Overview
This update implements a new feature that allows users to view quality issues that are either:
1. **In projects they have access to** (existing behavior)
2. **Assigned to them directly** (new behavior)

## Problem Statement
Previously, users could only see quality issues in projects they had access to. However, if a quality issue was assigned to a user but the user didn't have access to the project, they couldn't see the issue assigned to them.

## Solution
The system now uses an **OR** condition to filter quality issues:
- Users can see issues in projects they have access to **OR**
- Users can see issues that are assigned to them (via TeamMember assignment)

## Technical Implementation

### Files Modified

#### 1. `Dubox.Application/Specifications/GetQualityIssuesSpecification.cs`
**Changes:**
- Added `currentUserId` parameter to the constructor
- Added logic to filter by accessible projects OR assigned user:
  ```csharp
  if (accessibleProjectIds != null && currentUserId.HasValue)
  {
      // User can see quality issues that are either:
      // 1. In accessible projects, OR
      // 2. Assigned to them (via TeamMember)
      AddCriteria(q => 
          accessibleProjectIds.Contains(q.Box.ProjectId) || 
          (q.AssignedToMember != null && q.AssignedToMember.UserId == currentUserId.Value)
      );
  }
  ```

#### 2. `Dubox.Application/Specifications/GetQualityIssuesSummarySpecification.cs`
**Changes:**
- Added `currentUserId` parameter to the constructor
- Added necessary includes for `AssignedToMember` and `User`
- Applied the same OR filtering logic for summary calculations

#### 3. `Dubox.Application/Features/QualityIssues/Queries/GetQualityIssuesQueryHandler.cs`
**Changes:**
- Injected `ICurrentUserService` to get the current user's ID
- Retrieved the current user ID from the authentication context
- Passed `currentUserId` to both specifications (main query and summary)
- Updated the `CalculateSummaryAsync` method signature to include `currentUserId`

## How It Works

### Flow:
1. **User Authentication**: The system retrieves the current user's ID from `ICurrentUserService`
2. **Access Check**: The system determines which projects the user has access to via `IProjectTeamVisibilityService`
3. **Filtering**: The specification applies the following filter:
   - If user has limited project access AND has a valid user ID:
     - Show issues from accessible projects **OR** issues assigned to the user
   - If user is SystemAdmin/Viewer (null accessibleProjectIds):
     - Show all issues (no filtering)
4. **Summary Calculation**: The same filtering logic is applied when calculating summary statistics

### Data Relationship:
```
QualityIssue 
  ├── AssignedToMemberId (Guid?) 
  └── AssignedToMember (TeamMember)
        └── UserId (Guid)
```

The filtering checks if `QualityIssue.AssignedToMember.UserId` matches the current user's ID.

## User Roles & Access

### System Admin / Viewer
- **Access**: All quality issues (null accessibleProjectIds)
- **Behavior**: No filtering applied

### Project Manager / Team Member
- **Access**: Quality issues in accessible projects OR assigned to them
- **Behavior**: OR filtering applied

## Benefits
1. **Improved Task Visibility**: Users can now see all tasks assigned to them, even if they're not in their accessible projects
2. **Better Accountability**: Team members can track issues assigned to them across all projects
3. **Flexible Access Control**: Maintains project-based access control while allowing cross-project issue assignment

## Backward Compatibility
- The changes are **fully backward compatible**
- If `currentUserId` is not available, the system falls back to the original behavior (project-based filtering only)
- No frontend changes required - the API automatically returns the correct filtered results

## Testing Recommendations

### Test Cases:
1. **User with project access**: Verify they see issues in their projects
2. **User with assigned issues**: Verify they see issues assigned to them even if not in their projects
3. **System Admin**: Verify they see all issues
4. **User without assignments**: Verify they only see issues from accessible projects
5. **Summary Statistics**: Verify counts reflect both accessible project issues and assigned issues

### Test Scenarios:
- User A has access to Project X
- Issue 1 is in Project X (User A should see it)
- Issue 2 is in Project Y but assigned to User A (User A should see it)
- Issue 3 is in Project Y and not assigned to User A (User A should NOT see it)

## Performance Considerations
- The additional OR condition in the query is efficient as it uses indexed fields
- The `AssignedToMember` and `User` includes were already present in the original specification
- No additional database queries are needed

## Future Enhancements
Consider adding similar filtering for:
- Issues where user is CCed (via `CCUserId`)
- Issues created by the user
- Issues in projects where user is a team lead
