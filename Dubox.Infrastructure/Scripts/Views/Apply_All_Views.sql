-- =============================================================================
-- Master script – applies ALL database views in the correct order.
--
-- Usage (production):
--   sqlcmd -S <server> -d <database> -i Apply_All_Views.sql
--   -- or run it directly in SSMS against the target database.
--
-- All individual scripts use CREATE OR ALTER VIEW, so this file is
-- fully IDEMPOTENT – safe to run on a fresh database or as an update.
--
-- Naming convention for view scripts:
--   V<NNN>__<view_name>.sql
--   NNN  = three-digit sequence number (execution order)
-- =============================================================================

PRINT 'Applying views...';
GO

-- ---------------------------------------------------------------------------
-- V001 : vw_ActivityReviewsSummary
-- ---------------------------------------------------------------------------
PRINT '  V001 - vw_ActivityReviewsSummary';
GO
:r V001__vw_ActivityReviewsSummary.sql

-- ---------------------------------------------------------------------------
-- V002 : vw_ScheduleActivitiesForExport
-- ---------------------------------------------------------------------------
PRINT '  V002 - vw_ScheduleActivitiesForExport';
GO
:r V002__vw_ScheduleActivitiesForExport.sql

-- ---------------------------------------------------------------------------
-- V003 : vw_ScheduleActivitiesWithCounts
-- ---------------------------------------------------------------------------
PRINT '  V003 - vw_ScheduleActivitiesWithCounts';
GO
:r V003__vw_ScheduleActivitiesWithCounts.sql

-- ---------------------------------------------------------------------------
-- Add new view scripts here following the same pattern, e.g.:
-- PRINT '  V004 - vw_SomeOtherView';
-- GO
-- :r V004__vw_SomeOtherView.sql
-- ---------------------------------------------------------------------------

PRINT 'All views applied successfully.';
GO
