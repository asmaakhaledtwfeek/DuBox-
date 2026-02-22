-- =============================================================================
-- View  : dbo.vw_ScheduleActivitiesWithCounts
-- Purpose: Replaces the EF Include(AssignedTeams) + Include(AssignedMaterials)
--          pattern used in GetScheduleActivitiesByProjectQueryHandler.
--          Instead of loading every team/material row to count them in C#,
--          SQL Server pre-aggregates the counts so only one result set is
--          returned with the exact columns the ScheduleActivityListDto needs.
--
-- Columns (must match ordinal positions in the handler reader mapping):
--   0  ScheduleActivityId   UNIQUEIDENTIFIER
--   1  ParentActivityId     UNIQUEIDENTIFIER  NULL
--   2  OverallSequence      INT
--   3  ActivityCode         NVARCHAR(100)
--   4  ActivityName         NVARCHAR(200)
--   5  Stage                NVARCHAR(100)
--   6  StageNumber          INT
--   7  IsCustomActivity     BIT
--   8  PlannedStartDate     DATETIME2
--   9  PlannedFinishDate    DATETIME2
--  10  ActualStartDate      DATETIME2  NULL
--  11  ActualFinishDate     DATETIME2  NULL
--  12  Status               NVARCHAR(50)
--  13  PercentComplete      DECIMAL
--  14  Weight               DECIMAL
--  15  ProjectId            UNIQUEIDENTIFIER  NULL  (filter column)
--  16  TeamCount            INT
--  17  MaterialCount        INT
-- =============================================================================

CREATE OR ALTER VIEW [dbo].[vw_ScheduleActivitiesWithCounts]
AS
SELECT
    sa.ScheduleActivityId,
    sa.ParentActivityId,
    sa.OverallSequence,
    sa.ActivityCode,
    sa.ActivityName,
    sa.Stage,
    sa.StageNumber,
    sa.IsCustomActivity,
    sa.PlannedStartDate,
    sa.PlannedFinishDate,
    sa.ActualStartDate,
    sa.ActualFinishDate,
    sa.Status,
    sa.PercentComplete,
    sa.Weight,
    sa.ProjectId,
    ISNULL(tc.TeamCount,     0) AS TeamCount,
    ISNULL(mc.MaterialCount, 0) AS MaterialCount
FROM [dbo].[ScheduleActivities] sa
LEFT JOIN
(
    SELECT ScheduleActivityId, COUNT(*) AS TeamCount
    FROM   [dbo].[ScheduleActivityTeams]
    GROUP BY ScheduleActivityId
) tc ON sa.ScheduleActivityId = tc.ScheduleActivityId
LEFT JOIN
(
    SELECT ScheduleActivityId, COUNT(*) AS MaterialCount
    FROM   [dbo].[ScheduleActivityMaterials]
    GROUP BY ScheduleActivityId
) mc ON sa.ScheduleActivityId = mc.ScheduleActivityId;
GO
