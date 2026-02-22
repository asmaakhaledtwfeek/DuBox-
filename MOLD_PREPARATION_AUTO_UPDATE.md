# Mold Preparation Auto-Update Feature

## Overview
When a user marks the **Mold Preparation** stage as complete for a panel, the system automatically applies this completion to all other panels in the same project that have the same panel type.

## Implementation Details

### Location
- **File**: `Dubox.Application/Features/BoxPanels/Commands/UpdatePanelWorkflowStatusCommandHandler.cs`

### Key Changes

#### 1. New Helper Method
Added `ApplyMoldPreparationToSameTypePanels()` method that:
- Finds all panels in the same project with the same `PanelTypeId`
- Excludes the current panel being updated
- Only updates panels where `MoldPreparationComplete` is `false`
- Updates the following properties for matching panels:
  - `MoldPreparationComplete = true`
  - `MoldPreparationDate = statusTime`
  - `ModifiedDate = statusTime`
  - `ModifiedBy = currentUserId`
  - If panel status is `NotStarted`, updates to `InProgress`
  - If current stage is `NotStarted`, updates to `MoldPreparation`

#### 2. Integration Points
The auto-update logic is applied when Mold Preparation is marked complete through any of these scenarios:

1. **Direct Stage Selection**: User selects "Mold Preparation" stage
2. **Stage 2 (Initial)**: Implicitly marks Mold Preparation as complete
3. **Stage 3 (MEP Inserts Installation)**: Implicitly marks Mold Preparation as complete
4. **Stage 4 (Reinforcement Setup)**: Implicitly marks Mold Preparation as complete
5. **Stage 5 (Concrete Casting)**: Implicitly marks Mold Preparation as complete
6. **Stage 6 (Surface Finishing)**: Implicitly marks Mold Preparation as complete
7. **Stage 7 (Curing and Demolding)**: Implicitly marks Mold Preparation as complete
8. **Workflow Status "Completed"**: Marks all stages including Mold Preparation as complete

### Logic Flow

```
1. User marks a stage as complete (e.g., Mold Preparation)
   ↓
2. Check if panel has a PanelTypeId
   ↓
3. Query all panels in the same project with the same PanelTypeId
   ↓
4. Filter panels where MoldPreparationComplete = false
   ↓
5. Update each matching panel:
   - Mark Mold Preparation as complete
   - Set completion date
   - Update status to InProgress if NotStarted
   - Update stage to MoldPreparation if NotStarted
   ↓
6. Save all changes in a single transaction
```

## Business Rules

### When Auto-Update Applies
- Panel must have a `PanelTypeId` (panels without types are not affected)
- Only applies to panels in the **same project**
- Only updates panels where Mold Preparation is **not yet complete**
- Only triggers when Mold Preparation is being marked complete for the first time

### What Gets Updated
For all matching panels:
- ✅ Mold Preparation marked as complete
- ✅ Completion date set to current time
- ✅ Modified date and user tracked
- ✅ Panel status updated from NotStarted → InProgress (if applicable)
- ✅ Current stage updated to MoldPreparation (if still at NotStarted)

### What Does NOT Get Updated
- ❌ Panels from different projects
- ❌ Panels with different panel types
- ❌ Panels where Mold Preparation is already complete
- ❌ Panels without a PanelTypeId
- ❌ Other stages (Initial, MEP, Reinforcement, etc.) - only Mold Preparation is auto-updated

## Example Scenario

### Before
**Project**: High-Rise Building  
**Panel Type**: Wall-External-8"

| Panel Name | Panel Type | Mold Prep Complete | Status |
|------------|------------|-------------------|---------|
| Wall 01    | Wall-External-8" | ❌ No | NotStarted |
| Wall 02    | Wall-External-8" | ❌ No | NotStarted |
| Wall 03    | Wall-External-8" | ❌ No | NotStarted |
| Slab 01    | Slab-6" | ❌ No | NotStarted |

### Action
User marks **Mold Preparation** as complete for **Wall 01**

### After
| Panel Name | Panel Type | Mold Prep Complete | Status |
|------------|------------|-------------------|---------|
| Wall 01    | Wall-External-8" | ✅ Yes | InProgress |
| Wall 02    | Wall-External-8" | ✅ Yes (auto-updated) | InProgress |
| Wall 03    | Wall-External-8" | ✅ Yes (auto-updated) | InProgress |
| Slab 01    | Slab-6" | ❌ No (different type) | NotStarted |

## Benefits

1. **Efficiency**: Reduces repetitive data entry for panels of the same type
2. **Consistency**: Ensures all panels of the same type progress together through mold preparation
3. **Accuracy**: Eliminates human error in tracking mold completion
4. **Time Savings**: Especially beneficial for projects with many panels of the same type

## Technical Notes

- Uses Entity Framework's `ToListAsync()` to fetch matching panels
- Updates are performed in the same database transaction
- All changes are tracked with audit information (ModifiedBy, ModifiedDate)
- No additional API calls required - happens automatically in the backend

## Testing Recommendations

1. Test with panels of the same type in the same project
2. Test with panels of different types in the same project
3. Test with panels in different projects
4. Test with panels that already have Mold Preparation complete
5. Test when marking later stages (Initial, MEP, etc.)
6. Test when marking workflow as "Completed"

## Potential Future Enhancements

- Extend to other stages beyond Mold Preparation
- Add configuration to enable/disable per project
- Add notification when auto-update occurs
- Add bulk update summary in the response
- Add option to undo auto-updates
