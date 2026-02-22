-- =============================================================================
-- View  : dbo.vw_ScheduleActivitiesForExport
-- Purpose: Exposes only the columns required by the Excel-export handler,
--          avoiding the cost of loading unused columns (Description, WIRCode,
--          ApplicableBoxTypes, DependsOnActivities, Weight, audit fields, etc.)
--          across potentially thousands of schedule-activity rows.
--
-- Columns (must match ordinal positions in ExportScheduleActivitiesToExcelCommandHandler):
--   0  ScheduleActivityId   UNIQUEIDENTIFIER   PK
--   1  ParentActivityId     UNIQUEIDENTIFIER   NULL – hierarchy link
--   2  OverallSequence      INT                sort order
--   3  ActivityCode         NVARCHAR(100)
--   4  ActivityName         NVARCHAR(200)
--   5  PlannedStartDate     DATETIME2
--   6  PlannedFinishDate    DATETIME2
--   7  Stage                NVARCHAR(100)
--   8  Status               NVARCHAR(50)
--   9  PercentComplete      DECIMAL
--  10  ActualStartDate      DATETIME2          NULL
--  11  ActualFinishDate     DATETIME2          NULL
--  12  ProjectId            UNIQUEIDENTIFIER   NULL – used to filter by project
-- =============================================================================

CREATE OR ALTER VIEW [dbo].[vw_ScheduleActivitiesForExport]
AS
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
    ActualFinishDate,
    ProjectId
FROM [dbo].[ScheduleActivities];
GO
