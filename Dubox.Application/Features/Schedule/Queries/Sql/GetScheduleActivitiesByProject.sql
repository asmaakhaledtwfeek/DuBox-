-- Returns all schedule activities for a project with pre-computed team/material counts.
-- Queries vw_ScheduleActivitiesWithCounts – replaces the EF
-- Include(AssignedTeams) + Include(AssignedMaterials) eager loads.
--
-- Runtime usage : parameter is supplied by the ADO.NET handler (IDbQueryExecutor).
-- SSMS / manual  : replace the DECLARE block below with a real ProjectId from your DB.
--
-- Columns returned (by ordinal – must match the reader mapping in the handler):
--   0  ScheduleActivityId   UNIQUEIDENTIFIER
--   1  ParentActivityId     UNIQUEIDENTIFIER  NULL
--   2  OverallSequence      INT
--   3  ActivityCode         NVARCHAR
--   4  ActivityName         NVARCHAR
--   5  Stage                NVARCHAR
--   6  StageNumber          INT
--   7  IsCustomActivity     BIT
--   8  PlannedStartDate     DATETIME2
--   9  PlannedFinishDate    DATETIME2
--  10  ActualStartDate      DATETIME2  NULL
--  11  ActualFinishDate     DATETIME2  NULL
--  12  Status               NVARCHAR
--  13  PercentComplete      DECIMAL
--  14  Weight               DECIMAL
--  15  TeamCount            INT
--  16  MaterialCount        INT

-- ============================================================
-- SSMS / manual test block – remove or comment out in production
-- Replace the value below with a real ProjectId from your DB:
--   SELECT TOP 1 ProjectId FROM ScheduleActivities
DECLARE @projectId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
-- ============================================================
SELECT
    ScheduleActivityId,
    ParentActivityId,
    OverallSequence,
    ActivityCode,
    ActivityName,
    Stage,
    StageNumber,
    IsCustomActivity,
    PlannedStartDate,
    PlannedFinishDate,
    ActualStartDate,
    ActualFinishDate,
    Status,
    PercentComplete,
    Weight,
    TeamCount,
    MaterialCount
FROM [dbo].[vw_ScheduleActivitiesWithCounts]
WHERE ProjectId = @projectId
ORDER BY OverallSequence;
