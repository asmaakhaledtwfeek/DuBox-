-- =============================================
-- Script to Update Existing Panel Data for 7-Stage System
-- =============================================
-- This script migrates existing 4-stage panel data to the new 7-stage system
-- Run this AFTER applying the database migration that added the new columns

-- Step 1: Update InitialComplete based on MoldPreparationComplete
-- If Mold Preparation was complete, Initial should also be complete
UPDATE BoxPanels
SET 
    InitialComplete = CASE 
        WHEN MoldPreparationComplete = 1 THEN 1 
        ELSE 0 
    END,
    InitialDate = CASE 
        WHEN MoldPreparationComplete = 1 AND InitialDate IS NULL 
        THEN MoldPreparationDate 
        ELSE InitialDate 
    END
WHERE InitialComplete IS NULL OR InitialComplete = 0;

-- Step 2: Update MEPInsertsInstallationComplete based on ReinforcementSetupComplete
-- If Reinforcement Setup was complete, MEP Inserts should also be complete
UPDATE BoxPanels
SET 
    MEPInsertsInstallationComplete = CASE 
        WHEN ReinforcementSetupComplete = 1 THEN 1 
        ELSE 0 
    END,
    MEPInsertsInstallationDate = CASE 
        WHEN ReinforcementSetupComplete = 1 AND MEPInsertsInstallationDate IS NULL 
        THEN ReinforcementSetupDate 
        ELSE MEPInsertsInstallationDate 
    END
WHERE MEPInsertsInstallationComplete IS NULL OR MEPInsertsInstallationComplete = 0;

-- Step 3: Update SurfaceFinishingComplete based on ConcreteCastingComplete
-- If Concrete Casting was complete, Surface Finishing should also be complete
UPDATE BoxPanels
SET 
    SurfaceFinishingComplete = CASE 
        WHEN ConcreteCastingComplete = 1 THEN 1 
        ELSE 0 
    END,
    SurfaceFinishingDate = CASE 
        WHEN ConcreteCastingComplete = 1 AND SurfaceFinishingDate IS NULL 
        THEN ConcreteCastingDate 
        ELSE SurfaceFinishingDate 
    END
WHERE SurfaceFinishingComplete IS NULL OR SurfaceFinishingComplete = 0;

-- Step 4: Verify the update
SELECT 
    PanelName,
    WorkflowStatus,
    CurrentStage,
    MoldPreparationComplete,
    InitialComplete,
    MEPInsertsInstallationComplete,
    ReinforcementSetupComplete,
    ConcreteCastingComplete,
    SurfaceFinishingComplete,
    CuringAndDemoldingComplete,
    -- Calculate total completed stages
    (CAST(ISNULL(MoldPreparationComplete, 0) AS INT) +
     CAST(ISNULL(InitialComplete, 0) AS INT) +
     CAST(ISNULL(MEPInsertsInstallationComplete, 0) AS INT) +
     CAST(ISNULL(ReinforcementSetupComplete, 0) AS INT) +
     CAST(ISNULL(ConcreteCastingComplete, 0) AS INT) +
     CAST(ISNULL(SurfaceFinishingComplete, 0) AS INT) +
     CAST(ISNULL(CuringAndDemoldingComplete, 0) AS INT)) AS TotalStagesComplete
FROM BoxPanels
ORDER BY PanelName;

-- =============================================
-- Notes:
-- =============================================
-- 1. This script assumes:
--    - Old 4-stage system mapped to new 7-stage system as follows:
--      * Mold Preparation (stage 1) → Mold Preparation (stage 1) + Initial (stage 2)
--      * Reinforcement Setup (old stage 2) → MEP Inserts (stage 3) + Reinforcement Setup (stage 4)
--      * Concrete Casting (old stage 3) → Concrete Casting (stage 5) + Surface Finishing (stage 6)
--      * Curing & Demolding (old stage 4) → Curing & Demolding (stage 7)
--
-- 2. After running this script, all existing panels will have their new stage fields populated
--
-- 3. New panels created after this update will use the full 7-stage workflow
--
-- =============================================
