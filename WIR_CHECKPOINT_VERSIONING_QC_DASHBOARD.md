# WIR Checkpoint Versioning - Quality Control Dashboard

## Overview
Implemented checkpoint versioning in the Quality Control Dashboard to group and display multiple versions of the same WIR checkpoint for a specific box, similar to how it's implemented in the Box Details component.

## Implementation Date
January 20, 2026

## Changes Made

### 1. Backend Support (Already Exists)
The backend already has full support for checkpoint versioning:
- **WIRCheckpoint Entity** (`Dubox.Domain.Entities.WIRCheckpoint.cs`)
  - `Version` field (int) - defaults to 1
  - `ParentWIRId` field (Guid?) - reference to parent checkpoint
  - Navigation properties for parent and child versions
  
- **Frontend Model** (`dubox-frontend/src/app/core/models/wir.model.ts`)
  - `version?: number`
  - `parentWIRId?: string`
  - `newVersionId?: string`
  - `newVersionNumber?: number`

### 2. Frontend TypeScript Changes
**File**: `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.ts`

#### Added Properties:
```typescript
// Versioning support
expandedCheckpointVersions: Set<string> = new Set(); // Track expanded checkpoints
allCheckpointVersionsMap: Map<string, EnrichedCheckpoint[]> = new Map(); // Store all versions
```

#### Added Methods:

1. **`getLatestCheckpointsOnly(checkpoints)`**
   - Groups checkpoints by `wirNumber + boxId` composite key
   - Sorts versions by version number (descending) and creation date
   - Returns only the latest version for each group
   - Stores all versions in `allCheckpointVersionsMap`
   - Adds metadata: `_totalVersions` and `_groupKey`

2. **`hasMultipleVersions(checkpoint)`**
   - Checks if a checkpoint has multiple versions

3. **`getCheckpointVersionLabel(checkpoint)`**
   - Returns version label (e.g., "v2") if multiple versions exist
   - Returns null for single-version checkpoints

4. **`getTotalVersionCount(checkpoint)`**
   - Returns total number of versions for a checkpoint

5. **`toggleCheckpointVersions(checkpoint)`**
   - Toggles expansion/collapse of older versions

6. **`isCheckpointVersionsExpanded(checkpoint)`**
   - Checks if checkpoint versions are expanded

7. **`getOlderVersions(checkpoint)`**
   - Returns array of older versions (excluding the latest)

8. **`isOlderVersion(checkpoint, latestCheckpoint)`**
   - Checks if a checkpoint is an older version

### 3. Frontend HTML Template Changes
**File**: `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.html`

#### Table View Updates:
- Wrapped table rows in `<ng-container>` for grouping
- Added version badge next to WIR number
- Added expand/collapse button showing version count
- Added collapsible older version rows with:
  - Gray background to distinguish from current versions
  - Dashed border separation
  - "Historical Version" label
  - Read-only view (no action buttons)

#### Mobile Card View Updates:
- Added version badge in card header
- Added expand/collapse button
- Added collapsible older versions section
- Styled older versions with gray background

### 4. CSS Styling
**File**: `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.scss`

Added comprehensive styling for:
- Version badges (blue gradient for current, gray for historical)
- Expand/collapse buttons with chevron rotation
- Older version rows with dashed borders
- Mobile responsive versioning UI
- Hover effects and transitions

## How It Works

### Grouping Logic
1. Checkpoints are fetched from the backend API
2. Frontend groups checkpoints by `wirNumber + boxId` composite key
3. For each group:
   - Sort by version (descending) and creation date
   - Display only the latest version
   - Store all versions in memory
   - Show version count if multiple versions exist

### User Interaction
1. **Default View**: Shows only the latest version of each checkpoint with a version badge (e.g., "v2")
2. **Expansion**: Click "View All (n)" to expand and see older versions
3. **Older Versions**: Displayed below the latest version with:
   - Gray background
   - Historical version indicator
   - No action buttons (read-only)
4. **Collapse**: Click "Hide (n)" to collapse older versions

## Example Scenario

### Before (Without Versioning)
```
Stage-1 | Box-001 | ProjectA | Pending    | Jan 15, 2026
Stage-1 | Box-001 | ProjectA | Rejected   | Jan 10, 2026
Stage-1 | Box-001 | ProjectA | Approved   | Jan 5, 2026
```

### After (With Versioning)
```
Stage-1 v3 [View All (3)] | Box-001 | ProjectA | Pending | Jan 15, 2026
  ↓ (When expanded)
  └─ Stage-1 v2 | Box-001 | ProjectA | Rejected | Jan 10, 2026 [Historical]
  └─ Stage-1 v1 | Box-001 | ProjectA | Approved | Jan 5, 2026 [Historical]
```

## Benefits

1. **Cleaner Interface**: Reduces visual clutter by showing only the latest versions
2. **Version History**: Users can still access historical versions when needed
3. **Better Context**: Version badges provide clear indication of resubmissions
4. **Consistent UX**: Matches the versioning UI in Box Details component
5. **Scalability**: Works efficiently even with many checkpoint versions

## Testing Recommendations

1. **Single Version**: Verify checkpoints with only v1 don't show version badge
2. **Multiple Versions**: Verify grouping works correctly for same WIR+Box combination
3. **Expansion**: Test expand/collapse functionality
4. **Mobile View**: Test on mobile devices for proper responsive behavior
5. **Filtering**: Verify versioning works correctly with filters applied
6. **Pagination**: Verify versioning works across different pages

## Related Files

### Backend
- `Dubox.Domain.Entities.WIRCheckpoint.cs` - Entity with versioning fields
- `Dubox.Application.Features.WIRCheckpoints.Queries.GetWIRCheckpointsQueryHandler.cs` - Query handler
- `Dubox.Application.Specifications.GetWIRCheckpointsSpecification.cs` - Specification with ordering

### Frontend
- `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.ts`
- `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.html`
- `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.scss`
- `dubox-frontend/src/app/core/models/wir.model.ts` - Model definitions

## Notes

- The versioning system already exists in the backend and Box Details component
- This implementation extends the same pattern to the Quality Control Dashboard
- The grouping key is `wirNumber + boxId` to ensure checkpoints for the same WIR on different boxes are treated separately
- Pagination counts reflect the number of latest checkpoints (not total versions)
- Older versions are not actionable - they serve as historical reference only
