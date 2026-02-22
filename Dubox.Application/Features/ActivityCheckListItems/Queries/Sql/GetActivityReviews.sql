-- ============================================================
-- GetActivityReviews.sql
-- Paginated, fully server-side query for the Activity Reviews
-- list.  All heavy aggregation is done once via GROUP BY CTEs
-- instead of correlated sub-selects per row, which caused
-- timeouts on large datasets.
--
-- Parameters (passed as named ADO.NET parameters):
--   @ProjectId      UNIQUEIDENTIFIER  NULL = all projects
--   @BoxId          UNIQUEIDENTIFIER  NULL = all boxes
--   @BuildingNumber NVARCHAR(100)     NULL = all buildings
--   @Floor          NVARCHAR(100)     NULL = all floors
--   @BoxTypeId      INT               NULL = all box types
--   @ReviewStatus   NVARCHAR(20)      NULL = all statuses
--   @SearchTerm     NVARCHAR(200)     NULL = no text filter
--   @Skip           INT               rows to skip  (OFFSET)
--   @PageSize       INT               rows to take  (FETCH)
--
-- Result columns (by ordinal – must match reader mapping):
--   0  BoxActivityId        UNIQUEIDENTIFIER
--   1  ActivityName         NVARCHAR
--   2  ActivityCode         NVARCHAR
--   3  BoxId                UNIQUEIDENTIFIER
--   4  BoxTag               NVARCHAR
--   5  ProjectId            UNIQUEIDENTIFIER
--   6  ProjectCode          NVARCHAR
--   7  ProjectName          NVARCHAR
--   8  ProgressPercentage   DECIMAL
--   9  TotalItems           INT
--  10  ReviewedItems        INT
--  11  PassedItems          INT
--  12  FailedItems          INT
--  13  LastReviewedDate     DATETIME2 (nullable)
--  14  LastReviewedBy       NVARCHAR  (nullable)
--  15  ReviewStatus         NVARCHAR  ('Pending'|'Partial'|'Completed')
--  16  TotalCount           INT       (COUNT(*) OVER() – total rows matching all filters)
-- ============================================================

WITH

-- ── 1. Pre-aggregate all review counts per BoxActivity ──────────────────────
-- Single GROUP BY scan on ActivityCheckListItemReviews (no per-row sub-select).
ReviewAgg AS (
    SELECT
        r.BoxActivityId,
        SUM(CASE WHEN r.Status <> 0 THEN 1 ELSE 0 END) AS ReviewedItems,
        SUM(CASE WHEN r.Status =  1 THEN 1 ELSE 0 END) AS PassedItems,
        SUM(CASE WHEN r.Status =  2 THEN 1 ELSE 0 END) AS FailedItems,
        MAX(r.ReviewedDate)                              AS LastReviewedDate
    FROM ActivityCheckListItemReviews r
    GROUP BY r.BoxActivityId
),

-- ── 2. Pre-aggregate active checklist-item counts ───────────────────────────
-- Grouped by ActivityMasterId so a single JOIN resolves the count for
-- master-based activities.
ChecklistByMaster AS (
    SELECT
        aci.ActivityMasterId,
        COUNT(*)  AS TotalItems
    FROM ActivityCheckListItems aci
    WHERE aci.IsActive = 1
      AND aci.ActivityMasterId IS NOT NULL
    GROUP BY aci.ActivityMasterId
),

-- Grouped by ActivityTemplateActivityId for template-based activities.
ChecklistByTemplate AS (
    SELECT
        aci.ActivityTemplateActivityId,
        COUNT(*) AS TotalItems
    FROM ActivityCheckListItems aci
    WHERE aci.IsActive = 1
      AND aci.ActivityTemplateActivityId IS NOT NULL
    GROUP BY aci.ActivityTemplateActivityId
),

-- ── 3. Last-reviewer lookup (per BoxActivity) ───────────────────────────────
-- Resolved with ROW_NUMBER so we avoid a correlated sub-select per page row.
LastReviewer AS (
    SELECT
        r.BoxActivityId,
        COALESCE(u.FullName, u.Email) AS ReviewerName,
        ROW_NUMBER() OVER (
            PARTITION BY r.BoxActivityId
            ORDER BY r.ReviewedDate DESC
        ) AS rn
    FROM ActivityCheckListItemReviews r
    INNER JOIN Users u ON u.UserId = r.ReviewedBy
    WHERE r.ReviewedBy IS NOT NULL
),

-- ── 4. Main join – applies all dynamic WHERE filters ────────────────────────
Filtered AS (
    SELECT
        ba.BoxActivityId,
        COALESCE(am.ActivityName,  ata.ActivityName,  N'Unknown Activity') AS ActivityName,
        COALESCE(am.ActivityCode,  ata.ActivityCode,  N'Unknown')          AS ActivityCode,
        ba.BoxId,
        b.BoxTag,
        b.ProjectId,
        p.ProjectCode,
        p.ProjectName,
        ba.ProgressPercentage,
        COALESCE(ccm.TotalItems, cct.TotalItems, 0)   AS TotalItems,
        COALESCE(ra.ReviewedItems, 0)                  AS ReviewedItems,
        COALESCE(ra.PassedItems,   0)                  AS PassedItems,
        COALESCE(ra.FailedItems,   0)                  AS FailedItems,
        ra.LastReviewedDate,
        lr.ReviewerName                                AS LastReviewedBy,
        CASE
            WHEN COALESCE(ra.ReviewedItems, 0) = 0
                THEN N'Pending'
            WHEN COALESCE(ra.ReviewedItems, 0) >= COALESCE(ccm.TotalItems, cct.TotalItems, 0)
             AND      COALESCE(ccm.TotalItems, cct.TotalItems, 0) > 0
                THEN N'Completed'
            ELSE N'Partial'
        END AS ReviewStatus
    FROM       BoxActivities          ba
    INNER JOIN Boxes                  b   ON b.BoxId                       = ba.BoxId
    INNER JOIN Projects               p   ON p.ProjectId                   = b.ProjectId
    LEFT  JOIN ActivityMaster        am  ON am.ActivityMasterId            = ba.ActivityMasterId
    LEFT  JOIN ActivityTemplateActivities ata
                                          ON ata.ActivityTemplateActivityId = ba.ActivityTemplateActivityId
    LEFT  JOIN ReviewAgg              ra  ON ra.BoxActivityId               = ba.BoxActivityId
    LEFT  JOIN ChecklistByMaster      ccm ON ccm.ActivityMasterId           = ba.ActivityMasterId
    LEFT  JOIN ChecklistByTemplate    cct ON cct.ActivityTemplateActivityId = ba.ActivityTemplateActivityId
    LEFT  JOIN LastReviewer           lr  ON lr.BoxActivityId               = ba.BoxActivityId
                                         AND lr.rn                          = 1
    -- Only activities that have at least one active checklist item
    WHERE COALESCE(ccm.TotalItems, cct.TotalItems, 0) > 0

    -- ── Dynamic filter predicates ──────────────────────────────────────────
    AND (@ProjectId      IS NULL OR b.ProjectId          = @ProjectId)
    AND (@BoxId          IS NULL OR ba.BoxId             = @BoxId)
    AND (@BuildingNumber IS NULL OR b.BuildingNumber     = @BuildingNumber)
    AND (@Floor          IS NULL OR b.Floor              = @Floor)
    AND (@BoxTypeId      IS NULL OR b.ProjectBoxTypeId   = @BoxTypeId)
    AND (@ReviewStatus   IS NULL OR
         CASE
             WHEN COALESCE(ra.ReviewedItems, 0) = 0 THEN N'Pending'
             WHEN COALESCE(ra.ReviewedItems, 0) >= COALESCE(ccm.TotalItems, cct.TotalItems, 0)
              AND COALESCE(ccm.TotalItems, cct.TotalItems, 0) > 0 THEN N'Completed'
             ELSE N'Partial'
         END = @ReviewStatus)
    AND (@SearchTerm IS NULL OR
             am.ActivityName          LIKE N'%' + @SearchTerm + N'%'
          OR am.ActivityCode          LIKE N'%' + @SearchTerm + N'%'
          OR ata.ActivityName         LIKE N'%' + @SearchTerm + N'%'
          OR ata.ActivityCode         LIKE N'%' + @SearchTerm + N'%'
          OR b.BoxTag                 LIKE N'%' + @SearchTerm + N'%'
          OR p.ProjectCode            LIKE N'%' + @SearchTerm + N'%'
          OR p.ProjectName            LIKE N'%' + @SearchTerm + N'%')
)

-- ── 5. Final select: page data + total count in one round-trip ─────────────
-- COUNT(*) OVER() is computed across all filtered rows before OFFSET/FETCH,
-- so the frontend receives TotalCount without a second query.
SELECT
    BoxActivityId,
    ActivityName,
    ActivityCode,
    BoxId,
    BoxTag,
    ProjectId,
    ProjectCode,
    ProjectName,
    ProgressPercentage,
    TotalItems,
    ReviewedItems,
    PassedItems,
    FailedItems,
    LastReviewedDate,
    LastReviewedBy,
    ReviewStatus,
    COUNT(*) OVER () AS TotalCount
FROM Filtered
ORDER BY COALESCE(LastReviewedDate, '1900-01-01') DESC
OFFSET @Skip ROWS FETCH NEXT @PageSize ROWS ONLY;

