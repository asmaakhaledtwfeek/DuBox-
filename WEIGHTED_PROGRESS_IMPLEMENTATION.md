# Weighted Progress Implementation

## Overview

Implemented weighted progress calculation for Schedule Activities, replacing the simple average with a weighted average that respects activity importance.

## Changes Made

### 1. Database Schema (Entity + Migration)

**File:** `Dubox.Domain/Entities/ScheduleActivity.cs`
- Added `Weight` property (decimal 5,2, default 1.0)
- Used for weighted progress calculation when rolling up to parent activities

**File:** `Dubox.Infrastructure/Migrations/20260216170000_AddWeightToScheduleActivity.cs`
- Migration adds `Weight` column to `ScheduleActivities` table
- Default value: 1.0 (equal weight for all existing activities)

### 2. Backend Logic

**File:** `Dubox.Application/Features/Schedule/Commands/UpdateActivityProgressCommandHandler.cs`

#### Updated Bottom-Up Calculation (Weighted Average)

**Previous (Simple Average):**
```csharp
parent.PercentComplete = sum / count;
```

**Current (Weighted Average):**
```csharp
// Calculate weighted average: sum(child.Progress × child.Weight) / sum(child.Weight)
decimal weightedSum = 0;
decimal totalWeight = 0;
foreach (var child in directChildren)
{
    var childProgress = child.ScheduleActivityId == request.ScheduleActivityId
        ? request.PercentComplete
        : child.PercentComplete;
    var childWeight = child.Weight > 0 ? child.Weight : 1.0m;
    
    weightedSum += childProgress * childWeight;
    totalWeight += childWeight;
}
parent.PercentComplete = totalWeight > 0 ? Math.Round(weightedSum / totalWeight, 2) : 0;
```

**Benefits:**
- Activities can have different importance (e.g., Quality Management Plan = 0.7, Safety Program = 0.3)
- Precision: 2 decimal places to avoid rounding gaps
- Safety: Defaults to 1.0 if weight is 0 or negative

#### Auto-Sync Behavior

1. **Bottom-Up (Child → Parent):**
   - When a child activity's progress is updated
   - Parent progress is recalculated as weighted average of all direct children
   - Recursively updates all ancestors up to the root

2. **Top-Down (Parent → Children):**
   - When a parent activity's progress is updated
   - New progress is pushed down to ALL descendants (children, grandchildren, etc.)
   - **Exception:** Activities already at 100% are NOT changed (remain completed)

### 3. DTOs and API

**Files Updated:**
- `Dubox.Application/DTOs/ScheduleActivityDto.cs`
- `Dubox.Application/Features/Schedule/Queries/ScheduleActivityHierarchyBuilder.cs`

Added `Weight` to:
- `ScheduleActivityDto` (detailed view)
- `ScheduleActivityListDto` (hierarchy view)
- `ActivityNode` (internal builder class)

### 4. Frontend TypeScript Interfaces

**Files Updated:**
- `dubox-frontend/src/app/features/schedule/schedule-dashboard/schedule-dashboard.component.ts`
- `dubox-frontend/src/app/features/schedule/schedule-dashboard/activity-tree-node.component.ts`
- `dubox-frontend/src/app/core/models/activity-template.model.ts`

Added `weight: number` to all `ScheduleActivity` and `ActivityTreeNode` interfaces.

## Example Usage

### Scenario 1: Simple Equal Weights (Default)

**Activities:**
- Activity A: 50% progress, Weight = 1.0
- Activity B: 80% progress, Weight = 1.0

**Parent Progress:**
```
Parent = (50 × 1.0 + 80 × 1.0) / (1.0 + 1.0) = 130 / 2 = 65%
```

### Scenario 2: Weighted by Importance

**Activities:**
- Quality Management Plan: 60% progress, Weight = 0.7 (70% of parent's weight)
- Safety Program: 20% progress, Weight = 0.3 (30% of parent's weight)

**Parent Progress:**
```
Parent = (60 × 0.7 + 20 × 0.3) / (0.7 + 0.3) = (42 + 6) / 1.0 = 48%
```

### Scenario 3: Top-Down Push (From Image)

**Initial State:**
- PREPARATION & SUBMISSION: 60%
  - Construction Environmental...: 100%
  - Quality Management Plan: 15%
  - Safety Program: 20%
  - Baseline Programme: 50%

**User Action:** Set PREPARATION & SUBMISSION to 80%

**Result:**
- PREPARATION & SUBMISSION: 80%
  - Construction Environmental...: **100%** (unchanged, already completed)
  - Quality Management Plan: **80%** (updated)
  - Safety Program: **80%** (updated)
  - Baseline Programme: **80%** (updated)

## Precision & Rounding

- All percentages rounded to **2 decimal places**
- Example: 33.333... → 33.33%
- Prevents UI display issues and calculation drift

## Migration Instructions

1. **Apply Migration:**
   ```bash
   cd Dubox.Infrastructure
   dotnet ef database update --startup-project ../Dubox.Api/Dubox.Api.csproj
   ```

2. **Verify:**
   - All existing activities get `Weight = 1.0` by default
   - No data loss or recalculation needed

3. **Set Custom Weights (Optional):**
   - Update activities via API or SQL to assign custom weights
   - Example SQL:
   ```sql
   UPDATE ScheduleActivities 
   SET Weight = 0.7 
   WHERE ActivityCode = 'QMP-001';
   ```

## Testing Checklist

- [x] Entity has Weight property
- [x] Migration creates Weight column with default 1.0
- [x] Bottom-up calculation uses weighted average
- [x] Top-down preserves 100% completed activities
- [x] DTOs include weight in API responses
- [x] Frontend interfaces have weight property
- [x] Precision: 2 decimal places throughout
- [x] No linter errors

## Future Enhancements

1. **UI for Weight Management:**
   - Add weight input field in activity edit modal
   - Show weight in activity tree (optional column)

2. **Weight Validation:**
   - Warn if sibling weights don't sum to 1.0
   - Suggest automatic normalization

3. **Templates:**
   - Store weights in Activity Templates
   - Apply template weights when creating activities

4. **Reporting:**
   - Show weighted vs unweighted progress comparison
   - Export weight data in Excel

## Summary

This implementation provides:
- ✅ **Weighted Progress:** Respects activity importance
- ✅ **Auto-Sync:** Bidirectional update (bottom-up + top-down)
- ✅ **Precision:** 2 decimal places, no rounding gaps
- ✅ **Smart Top-Down:** Preserves completed (100%) activities
- ✅ **Default Behavior:** Weight = 1.0 maintains simple average for existing data
