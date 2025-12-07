# Teams Performance Report - API Endpoints

This document outlines the recommended API endpoints needed to support the Teams Performance Report feature.

## Base Endpoint
All endpoints are under: `/api/reports/teams-performance`

## Required Endpoints

### 1. Get Teams Performance Report (Paginated)
**Endpoint:** `GET /api/reports/teams-performance`

**Query Parameters:**
- `page` (optional, int): Page number (default: 1)
- `pageSize` (optional, int): Items per page (default: 25)
- `projectId` (optional, Guid): Filter by project ID
- `teamId` (optional, Guid): Filter by team ID
- `status` (optional, int): Filter by activity status (1=NotStarted, 2=InProgress, 3=Completed, 4=OnHold, 5=Delayed, 6=Dispatched)
- `search` (optional, string): Search by team name

**Response:**
```json
{
  "data": {
    "items": [
      {
        "teamId": "guid",
        "teamName": "string",
        "membersCount": 0,
        "totalAssignedActivities": 0,
        "completed": 0,
        "inProgress": 0,
        "pending": 0,
        "delayed": 0,
        "averageTeamProgress": 0.0,
        "workloadLevel": "Low" | "Normal" | "Overloaded"
      }
    ],
    "page": 1,
    "pageSize": 25,
    "totalCount": 0,
    "totalPages": 0
  }
}
```

**Business Logic:**
- Calculate `membersCount` from TeamMembers count
- Calculate `totalAssignedActivities` from BoxActivities where TeamId matches
- Calculate `completed` from activities with Status = Completed
- Calculate `inProgress` from activities with Status = InProgress
- Calculate `pending` from activities with Status = NotStarted
- Calculate `delayed` from activities with Status = Delayed
- Calculate `averageTeamProgress` as average of ProgressPercentage for all team activities
- Calculate `workloadLevel`:
  - Low: activities per member < 3
  - Normal: activities per member 3-7
  - Overloaded: activities per member > 7

---

### 2. Get Teams Performance Summary (KPIs)
**Endpoint:** `GET /api/reports/teams-performance/summary`

**Query Parameters:**
- `projectId` (optional, Guid): Filter by project ID
- `teamId` (optional, Guid): Filter by team ID
- `status` (optional, int): Filter by activity status
- `search` (optional, string): Search by team name

**Response:**
```json
{
  "data": {
    "totalTeams": 0,
    "totalTeamMembers": 0,
    "totalAssignedActivities": 0,
    "completedActivities": 0,
    "inProgressActivities": 0,
    "delayedActivities": 0,
    "averageTeamProgress": 0.0,
    "teamWorkloadIndicator": 0.0
  }
}
```

**Business Logic:**
- `totalTeams`: Count of distinct teams with assigned activities
- `totalTeamMembers`: Sum of all team members across all teams
- `totalAssignedActivities`: Total count of activities assigned to teams
- `completedActivities`: Count of activities with Status = Completed
- `inProgressActivities`: Count of activities with Status = InProgress
- `delayedActivities`: Count of activities with Status = Delayed
- `averageTeamProgress`: Average of all team average progress percentages
- `teamWorkloadIndicator`: Average activities per team (totalAssignedActivities / totalTeams)

---

### 3. Get Team Activities (Drill-down)
**Endpoint:** `GET /api/reports/teams-performance/{teamId}/activities`

**Path Parameters:**
- `teamId` (required, Guid): Team ID

**Query Parameters:**
- `projectId` (optional, Guid): Filter by project ID
- `status` (optional, int): Filter by activity status

**Response:**
```json
{
  "data": {
    "teamId": "guid",
    "teamName": "string",
    "activities": [
      {
        "activityId": "guid",
        "activityName": "string",
        "boxTag": "string",
        "projectName": "string",
        "status": "string",
        "progressPercentage": 0.0,
        "plannedStartDate": "2024-01-01T00:00:00Z",
        "plannedEndDate": "2024-01-01T00:00:00Z",
        "actualStartDate": "2024-01-01T00:00:00Z",
        "actualEndDate": "2024-01-01T00:00:00Z",
        "duration": 0,
        "boxId": "guid",
        "projectId": "guid"
      }
    ],
    "totalCount": 0
  }
}
```

**Business Logic:**
- Return all BoxActivities assigned to the specified team
- Include Box and Project information for each activity
- Calculate `duration` from PlannedStartDate to PlannedEndDate (in days)
- Apply filters if provided

---

### 4. Export Teams Performance Report to Excel
**Endpoint:** `GET /api/reports/teams-performance/export/excel`

**Query Parameters:**
- `projectId` (optional, Guid): Filter by project ID
- `teamId` (optional, Guid): Filter by team ID
- `status` (optional, int): Filter by activity status
- `search` (optional, string): Search by team name

**Response:**
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- File download with filename: `teams_performance_report_{date}.xlsx`

**Excel Format:**
- Sheet name: "Teams Performance"
- Columns:
  1. Team Name
  2. Members Count
  3. Total Assigned Activities
  4. Completed
  5. In-Progress
  6. Pending
  7. Delayed
  8. Average Team Progress (%)
  9. Workload Level

---

## Database Queries Recommendations

### For Teams Performance Report:
```sql
SELECT 
    t.TeamId,
    t.TeamName,
    COUNT(DISTINCT tm.TeamMemberId) AS MembersCount,
    COUNT(ba.BoxActivityId) AS TotalAssignedActivities,
    SUM(CASE WHEN ba.Status = 3 THEN 1 ELSE 0 END) AS Completed,
    SUM(CASE WHEN ba.Status = 2 THEN 1 ELSE 0 END) AS InProgress,
    SUM(CASE WHEN ba.Status = 1 THEN 1 ELSE 0 END) AS Pending,
    SUM(CASE WHEN ba.Status = 5 THEN 1 ELSE 0 END) AS Delayed,
    AVG(ba.ProgressPercentage) AS AverageTeamProgress
FROM Teams t
LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId
LEFT JOIN BoxActivities ba ON t.TeamId = ba.TeamId
WHERE t.IsActive = 1
    AND ba.IsActive = 1
    AND (@ProjectId IS NULL OR ba.BoxId IN (SELECT BoxId FROM Boxes WHERE ProjectId = @ProjectId))
    AND (@TeamId IS NULL OR t.TeamId = @TeamId)
    AND (@Status IS NULL OR ba.Status = @Status)
GROUP BY t.TeamId, t.TeamName
ORDER BY t.TeamName
```

### For Summary KPIs:
```sql
SELECT 
    COUNT(DISTINCT t.TeamId) AS TotalTeams,
    COUNT(DISTINCT tm.TeamMemberId) AS TotalTeamMembers,
    COUNT(ba.BoxActivityId) AS TotalAssignedActivities,
    SUM(CASE WHEN ba.Status = 3 THEN 1 ELSE 0 END) AS CompletedActivities,
    SUM(CASE WHEN ba.Status = 2 THEN 1 ELSE 0 END) AS InProgressActivities,
    SUM(CASE WHEN ba.Status = 5 THEN 1 ELSE 0 END) AS DelayedActivities,
    AVG(ba.ProgressPercentage) AS AverageTeamProgress
FROM Teams t
LEFT JOIN TeamMembers tm ON t.TeamId = tm.TeamId
LEFT JOIN BoxActivities ba ON t.TeamId = ba.TeamId
WHERE t.IsActive = 1
    AND ba.IsActive = 1
    AND (@ProjectId IS NULL OR ba.BoxId IN (SELECT BoxId FROM Boxes WHERE ProjectId = @ProjectId))
    AND (@TeamId IS NULL OR t.TeamId = @TeamId)
    AND (@Status IS NULL OR ba.Status = @Status)
```

---

## Status Enum Values
- 1 = NotStarted
- 2 = InProgress
- 3 = Completed
- 4 = OnHold
- 5 = Delayed
- 6 = Dispatched

---

## Notes
- All endpoints should support pagination where applicable
- All endpoints should support filtering by project, team, and status
- Excel export should include all data (no pagination limit)
- Consider caching summary data for better performance
- Ensure proper authorization checks are in place

