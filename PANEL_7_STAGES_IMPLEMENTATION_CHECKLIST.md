# Panel 7-Stage System Implementation Checklist

## Overview
This document tracks the complete transformation from 4-stage to 7-stage panel workflow system.

---

## ✅ Completed Changes

### 1. Database Schema (Backend)

#### Domain Entities
- ✅ **BoxPanel.cs** - Added 3 new stage fields:
  - `InitialComplete` + `InitialDate`
  - `MEPInsertsInstallationComplete` + `MEPInsertsInstallationDate`
  - `SurfaceFinishingComplete` + `SurfaceFinishingDate`

#### Enums
- ✅ **PanelStageEnum.cs** - Updated to 7 stages:
  1. MoldPreparation = 1
  2. Initial = 2
  3. MEPInsertsInstallation = 3
  4. ReinforcementSetup = 4
  5. ConcreteCasting = 5
  6. SurfaceFinishing = 6
  7. CuringAndDemolding = 7

#### DTOs
- ✅ **BoxPanelDto.cs** - Added new stage completion fields

#### Command Handlers
- ✅ **UpdatePanelWorkflowStatusCommandHandler.cs** - Updated to handle all 7 stages
- ✅ **BoxMapper.cs** - Updated panel mapping to include all 7 stage fields

### 2. Frontend (Angular)

#### Models
- ✅ **panel-stage.model.ts** - Updated PanelStage enum and PANEL_STAGES array
- ✅ **box.model.ts** - Updated BoxPanel interface with new stage fields

#### Components
- ✅ **panel-workflow-modal.component.ts** - Updated for 7 stages:
  - `isStageComplete()` checks all 7 stages
  - `getCompletedStagesCount()` counts all 7 stages
  - `canSelectStage()` controls clickability (only when InProgress)

- ✅ **panel-workflow-modal.component.html** - Updated UI:
  - Shows all 7 stages by default
  - Stages clickable only when status is "InProgress"
  - Updated progress display (X/7 instead of X/4)

- ✅ **panel-workflow-modal.component.scss** - Added disabled state styling

- ✅ **box-panels.component.ts** - Updated stage counting
- ✅ **box-panels.component.html** - Updated to show "X/7 stages"

### 3. Navigation & UX
- ✅ **Warning Triangle Navigation** - Correctly navigates to Quality Issues tab and highlights
- ✅ **Workflow Update** - Stays on current tab, no unwanted navigation
- ✅ **Stage Clickability** - Only clickable when status is "InProgress"

---

## 🔧 Post-Migration Steps

### Step 1: Update Existing Panel Data
Run the SQL script to update existing panels:

```bash
# File location: UPDATE_PANEL_STAGES_DATA.sql
```

This script will:
- Set `InitialComplete` based on `MoldPreparationComplete`
- Set `MEPInsertsInstallationComplete` based on `ReinforcementSetupComplete`
- Set `SurfaceFinishingComplete` based on `ConcreteCastingComplete`
- Set appropriate dates for each stage

### Step 2: Rebuild Backend
```bash
cd "Dubox.Api"
dotnet build
```

### Step 3: Rebuild Frontend
```bash
cd dubox-frontend
npm run build
```

### Step 4: Test the Changes

#### Test Case 1: View Existing Panels
1. Navigate to a Box with panels
2. Go to "Panel Approvals" tab
3. Verify stage count shows "X/7 stages" (not X/4)

#### Test Case 2: Update Panel Workflow (InProgress)
1. Click "Workflow" button on a panel
2. Select "In Progress" status
3. Verify all 7 stages are visible and clickable
4. Select stage 6 (Surface Finishing)
5. Click "Update Status"
6. Verify you stay on Panels tab (no navigation)
7. Verify stage 6 is saved correctly in database

#### Test Case 3: Update Panel Workflow (OnHold/Completed)
1. Click "Workflow" button on a panel
2. Select "Completed" or "Put On Hold" status
3. Verify all 7 stages are visible but NOT clickable (disabled/dimmed)
4. Verify stages show completion status correctly

#### Test Case 4: Quality Issue Navigation
1. Find a panel with a quality issue (warning triangle ⚠️)
2. Click the warning triangle
3. Verify it navigates to Quality Issues tab
4. Verify the specific issue is highlighted with amber background
5. Verify smooth scroll to the issue

---

## 📋 Stage Mapping Reference

### New 7-Stage System:
1. **Mold Preparation** - Prepare and clean molds for casting
2. **Initial** - Initial stage - panel setup and preparation
3. **MEP Inserts / Embedded Items Installation** - Install MEP inserts and embedded items
4. **Reinforcement Setup** - Install steel reinforcement and fixtures
5. **Concrete Casting** - Pour and compact concrete
6. **Surface Finishing** - Apply surface finishing and treatment
7. **Curing & Demolding** - Cure concrete and remove from mold

### Database Fields:
- `MoldPreparationComplete` + `MoldPreparationDate`
- `InitialComplete` + `InitialDate`
- `MEPInsertsInstallationComplete` + `MEPInsertsInstallationDate`
- `ReinforcementSetupComplete` + `ReinforcementSetupDate`
- `ConcreteCastingComplete` + `ConcreteCastingDate`
- `SurfaceFinishingComplete` + `SurfaceFinishingDate`
- `CuringAndDemoldingComplete` + `CuringAndDemoldingDate`

---

## 🐛 Troubleshooting

### Issue: Stages still show "X/4" instead of "X/7"
**Solution:** 
1. Clear browser cache
2. Rebuild frontend: `npm run build`
3. Restart Angular dev server

### Issue: Stage 6 not saving to database
**Solution:**
1. Verify migration was applied: Check database for new columns
2. Run UPDATE_PANEL_STAGES_DATA.sql script
3. Verify backend code includes all 7 stages in UpdatePanelWorkflowStatusCommandHandler.cs

### Issue: Old panels showing incorrect stage counts
**Solution:**
1. Run UPDATE_PANEL_STAGES_DATA.sql to backfill data
2. Refresh the page to reload panel data from backend

### Issue: Stages are clickable when they shouldn't be
**Solution:**
1. Verify workflow status is set correctly
2. Check that `canSelectStage()` returns `false` when not "InProgress"
3. Verify `.disabled` class is applied in HTML template

---

## 🎯 Verification SQL Queries

### Check if new columns exist:
```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BoxPanels'
  AND COLUMN_NAME IN (
    'InitialComplete', 'InitialDate',
    'MEPInsertsInstallationComplete', 'MEPInsertsInstallationDate',
    'SurfaceFinishingComplete', 'SurfaceFinishingDate'
  );
```

### Check panel stage completion:
```sql
SELECT 
    PanelName,
    WorkflowStatus,
    MoldPreparationComplete,
    InitialComplete,
    MEPInsertsInstallationComplete,
    ReinforcementSetupComplete,
    ConcreteCastingComplete,
    SurfaceFinishingComplete,
    CuringAndDemoldingComplete,
    -- Calculate total
    (CAST(ISNULL(MoldPreparationComplete, 0) AS INT) +
     CAST(ISNULL(InitialComplete, 0) AS INT) +
     CAST(ISNULL(MEPInsertsInstallationComplete, 0) AS INT) +
     CAST(ISNULL(ReinforcementSetupComplete, 0) AS INT) +
     CAST(ISNULL(ConcreteCastingComplete, 0) AS INT) +
     CAST(ISNULL(SurfaceFinishingComplete, 0) AS INT) +
     CAST(ISNULL(CuringAndDemoldingComplete, 0) AS INT)) AS TotalComplete
FROM BoxPanels
ORDER BY PanelName;
```

---

## ✅ Implementation Complete!

All code changes have been made. Follow the post-migration steps above to complete the transformation.
