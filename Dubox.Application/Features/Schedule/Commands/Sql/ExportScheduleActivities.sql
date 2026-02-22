-- Returns all schedule-activity rows needed to build the Excel export for a given project.
-- Queries the pre-computed view vw_ScheduleActivitiesForExport (export-only columns,
-- no unused heavy fields).
--
-- Parameter:
--   @projectId  UNIQUEIDENTIFIER  – filters to a single project
--
-- ============================================================
-- SSMS / manual test block – replace with a real ProjectId:
--   SELECT TOP 1 ProjectId FROM ScheduleActivities
DECLARE @projectId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
-- ============================================================
--
-- Columns returned (by ordinal – must match the reader mapping in the handler):
--   0  ScheduleActivityId   UNIQUEIDENTIFIER
--   1  ParentActivityId     UNIQUEIDENTIFIER  NULL
--   2  OverallSequence      INT
--   3  ActivityCode         NVARCHAR
--   4  ActivityName         NVARCHAR
--   5  PlannedStartDate     DATETIME2
--   6  PlannedFinishDate    DATETIME2
--   7  Stage                NVARCHAR
--   8  Status               NVARCHAR
--   9  PercentComplete      DECIMAL
--  10  ActualStartDate      DATETIME2  NULL
--  11  ActualFinishDate     DATETIME2  NULL
SELECT
    ScheduleActivityId,
    ParentActivityId,
    OverallSequence,
    ActivityCode,
    ActivityName,
    PlannedStartDate,
    PlannedFinishDate,
    Stage,
    Status,
    PercentComplete,
    ActualStartDate,
    ActualFinishDate
FROM [dbo].[vw_ScheduleActivitiesForExport]
WHERE ProjectId = @projectId
ORDER BY OverallSequence;
