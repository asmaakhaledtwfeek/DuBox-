-- =============================================================================
-- View  : dbo.vw_ActivityReviewsSummary
-- Purpose: Pre-aggregates checklist item counts and reviewed item counts
--          per BoxActivity so the summary query needs only a single
--          COUNT / SUM pass instead of loading rows into the application.
--
-- Columns:
--   BoxActivityId   – the activity being tracked
--   TotalItems      – number of ACTIVE checklist items linked to this activity
--   ReviewedItems   – number of reviews whose Status is NOT Pending (i.e. Pass / Fail / NA)
--
-- Notes:
--   * Uses CREATE OR ALTER so the script is safe to re-run (idempotent).
--   * Status = 1 is CheckListItemStatusEnum.Pending.
--   * The INNER JOIN on ChecklistCounts intentionally filters out BoxActivities
--     that have no matching checklist items (mirrors the original LINQ filter
--     "where checklistItem != null && checklistItem.IsActive").
--   * NULL-safe join on both nullable FK columns (ActivityMasterId,
--     ActivityTemplateActivityId) so that rows where both sides are NULL
--     still match correctly.
-- =============================================================================

CREATE OR ALTER VIEW [dbo].[vw_ActivityReviewsSummary]
AS
WITH ChecklistCounts AS
(
    -- Count active checklist items grouped by the activity they belong to.
    SELECT
        ActivityMasterId,
        ActivityTemplateActivityId,
        COUNT(*) AS TotalItems
    FROM [dbo].[ActivityCheckListItems]
    WHERE IsActive = 1
    GROUP BY
        ActivityMasterId,
        ActivityTemplateActivityId
),
ReviewCounts AS
(
    -- Count non-Pending reviews grouped by BoxActivity.
    SELECT
        BoxActivityId,
        COUNT(*) AS ReviewedItems
    FROM [dbo].[ActivityCheckListItemReviews]
    WHERE Status <> 1          -- 1 = CheckListItemStatusEnum.Pending
    GROUP BY BoxActivityId
)
SELECT
    ba.BoxActivityId,
    cc.TotalItems,
    ISNULL(rc.ReviewedItems, 0) AS ReviewedItems
FROM [dbo].[BoxActivities]          ba
INNER JOIN ChecklistCounts          cc
    ON  (   ba.ActivityMasterId            = cc.ActivityMasterId
            OR (ba.ActivityMasterId        IS NULL AND cc.ActivityMasterId IS NULL)
        )
    AND (   ba.ActivityTemplateActivityId  = cc.ActivityTemplateActivityId
            OR (ba.ActivityTemplateActivityId IS NULL AND cc.ActivityTemplateActivityId IS NULL)
        )
LEFT JOIN ReviewCounts              rc
    ON  ba.BoxActivityId = rc.BoxActivityId;
GO
