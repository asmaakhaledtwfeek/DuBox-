-- Returns a single aggregated row with review-status counts for all BoxActivities
-- that have at least one active checklist item.
-- Queries the pre-computed view vw_ActivityReviewsSummary.
--
-- Columns returned (by ordinal – must match the reader mapping in the handler):
--   0  Total      – total number of BoxActivities with checklist items
--   1  Pending    – activities with no reviewed items yet
--   2  Completed  – activities where all items have been reviewed
--   3  InProgress – activities partially reviewed
SELECT
    COUNT(*)                                                                              AS Total,
    SUM(CASE WHEN ReviewedItems = 0                              THEN 1 ELSE 0 END)       AS Pending,
    SUM(CASE WHEN ReviewedItems >= TotalItems AND TotalItems > 0 THEN 1 ELSE 0 END)       AS Completed,
    COUNT(*)
        - SUM(CASE WHEN ReviewedItems = 0                              THEN 1 ELSE 0 END)
        - SUM(CASE WHEN ReviewedItems >= TotalItems AND TotalItems > 0 THEN 1 ELSE 0 END) AS InProgress
FROM [dbo].[vw_ActivityReviewsSummary];
