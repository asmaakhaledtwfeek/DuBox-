# WIR Position Lock Fix - Multiple Checkpoint Versions

## Issue Description

When a WIR has more than one checkpoint (versions created after rejection and resubmission), the position lock logic was checking the FIRST checkpoint found instead of the LATEST checkpoint version. This caused the position to remain locked even when the latest checkpoint was approved.

### Example Scenario:
- WIR-3 v1: **Rejected** (old version)
- WIR-3 v2: **Approved** (latest version)
- **Bug**: Position was locked because the system was checking v1 (rejected)
- **Expected**: Position should be unlocked because v2 (latest) is approved

## Root Cause

The code was using `.find()` which returns the FIRST matching checkpoint, not the LATEST one:

```typescript
// BEFORE (Incorrect):
const checkpoint = checkpoints.find(cp => 
  cp.wirNumber && cp.wirNumber.toUpperCase() === wirCode.toUpperCase()
);
```

This doesn't account for multiple checkpoint versions. When a WIR is rejected and resubmitted, a new version is created with an incremented version number (v1, v2, v3, etc.).

## Solution

Created a helper function `getLatestCheckpoint()` that:
1. Filters all checkpoints matching the WIR code
2. Sorts by version number in descending order
3. Returns the checkpoint with the highest version number (latest)

```typescript
// AFTER (Correct):
private getLatestCheckpoint(checkpoints: WIRCheckpoint[], wirCode: string): WIRCheckpoint | null {
  if (!checkpoints || !wirCode) return null;
  
  const matchingCheckpoints = checkpoints.filter(cp => 
    cp.wirNumber && cp.wirNumber.toUpperCase() === wirCode.toUpperCase()
  );
  
  if (matchingCheckpoints.length === 0) return null;
  
  // Sort by version descending and return the first one (latest version)
  matchingCheckpoints.sort((a, b) => (b.version || 1) - (a.version || 1));
  
  return matchingCheckpoints[0];
}
```

## Visual Example - Version Badge & Expansion

### Latest Version (Collapsed)
When a checkpoint has multiple versions, the UI displays:

```
┌──────────────────────────────────────────────────────────┐
│ Stage-3 [v4]     ● Approved     [▶ View All (4)]        │
│ ──────────────────────────────────────────────────────── │
│ Stage checkpoint for 8 Activities: Assembly &...         │
│                                                           │
│ 📅 Requested: Jan 19, 2026                               │
│ 👤 Inspector: Mamdouh Salem                              │
│ ✓ Checklist Items: 3 items                               │
│                                                           │
│ [+ Add Checklist] [Review] [Print] [Download]            │
└──────────────────────────────────────────────────────────┘
```

### All Versions (Expanded)
When "View All Versions" button in header is clicked:

```
┌──────────────────────────────────────────────────────────┐
│ Stage-3 [v4]     ● Approved     [▼ Hide (4)]            │
│ ──────────────────────────────────────────────────────── │
│ Stage checkpoint for 8 Activities: Assembly &...         │
│                                                           │
│ 📅 Requested: Jan 19, 2026 | Mamdouh Salem               │
│ 👤 Inspector: Mamdouh Salem | QC Engineer                │
│ ✓ Checklist Items: 3 items to review                     │
│ 🕐 Last Update: Jan 19, 2026 | Approved                  │
│                                                           │
│ [+ Add Checklist] [Review] [Print] [Download]            │
│ ══════════════════════════════════════════════════════   │
│                                                      │
│ ┌─ Historical Version ──────────────────────────┐   │
│ │ Stage-3 [v3]                   ● Rejected     │   │
│ │ ──────────────────────────────────────────────│   │
│ │ Stage checkpoint for 8 Activities: Assembly...│   │
│ │                                               │   │
│ │ 📅 Requested: Jan 18, 2026 | Mamdouh Salem   │   │
│ │ 👤 Inspector: Mamdouh Salem | QC Engineer    │   │
│ │ ✓ Checklist Items: 3 items to review         │   │
│ │ 🕐 Last Update: Jan 18, 2026 | Rejected      │   │
│ │                                               │   │
│ │ [Print] [Download]    🔒 Historical (Read-Only) │
│ └───────────────────────────────────────────────┘   │
│                                                      │
│ ┌─ Historical Version ──────────────────────────┐   │
│ │ Stage-3 [v2]                   ● Rejected     │   │
│ │ ──────────────────────────────────────────────│   │
│ │ Stage checkpoint for 8 Activities: Assembly...│   │
│ │                                               │   │
│ │ 📅 Requested: Jan 17, 2026 | Mamdouh Salem   │   │
│ │ 👤 Inspector: Mamdouh Salem | QC Engineer    │   │
│ │ ✓ Checklist Items: 3 items to review         │   │
│ │ 🕐 Last Update: Jan 17, 2026 | Rejected      │   │
│ │                                               │   │
│ │ [Print] [Download]    🔒 Historical (Read-Only) │
│ └───────────────────────────────────────────────┘   │
│                                                      │
│ ┌─ Historical Version ──────────────────────────┐   │
│ │ Stage-3 [v1]                   ● Rejected     │   │
│ │ ──────────────────────────────────────────────│   │
│ │ Stage checkpoint for 8 Activities: Assembly...│   │
│ │                                               │   │
│ │ 📅 Requested: Jan 16, 2026 | Mamdouh Salem   │   │
│ │ 👤 Inspector: Mamdouh Salem | QC Engineer    │   │
│ │ ✓ Checklist Items: 3 items to review         │   │
│ │ 🕐 Last Update: Jan 16, 2026 | Rejected      │   │
│ │                                               │   │
│ │ [Print] [Download]    🔒 Historical (Read-Only) │
│ └───────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

**Legend:**
- "Stage-3" = Checkpoint number (teal gradient background)
- "[v4]" = Latest version badge (blue)
  - Hover tooltip: "Version 4 of 4"
- "● Approved" = Status badge (green with pulsing dot)
- "[▶ View All (4)]" = Expandable button in header (blue, only visible when versions > 1)
  - Positioned on the right side of the header
  - Chevron rotates down when expanded
  - Text changes to "Hide (4)" when expanded
- Older versions: Gray badge, limited to Print/Download only

**Key Features:**
- **Latest version (v4)**: Full checkpoint card with all actions (Add Checklist, Review, Print, Download)
- **Older versions (v1-v3)**: Full checkpoint cards showing complete data:
  - Same card design as latest version
  - All checkpoint information visible (requested date, inspector, checklist items, etc.)
  - Gray/muted styling to indicate historical status
  - "Historical Version (Read-Only)" badge with lock icon
  - Limited to Print and Download actions only
- Version badge appears for all versions > 1
- Expandable section conserves space when collapsed
- Clear visual distinction through styling (latest = teal gradient, older = gray gradient)

## Files Modified

### 1. `update-progress-modal.component.ts`
- **Added**: `getLatestCheckpoint()` helper method
- **Updated 3 locations** to use the helper method:
  - `checkWIRStatusAndPreviousWIRForLocking()` - Line ~608
  - `checkPreviousWIRForLocking()` - Line ~770
  - `loadWIRCheckpoint()` - Line ~999

### 2. `wir.service.ts`
- **Updated**: `getWIRCheckpointByActivity()` method to:
  - Filter all matching checkpoints
  - Sort by version descending
  - Return the latest version

### 3. `activity-table.component.ts` ⚠️ CRITICAL FIX
- **Fixed**: `getCheckpoint()` method (Line 931)
  - **Bug**: Was returning `versions[versions.length - 1]` (oldest version)
  - **Fix**: Now returns `versions[0]` (latest version)
  - **Impact**: WIR status in activity table now correctly displays the status of the latest checkpoint version

### 4. `box-details.component.ts` - Stage Checkpoints List
- **Added**: `getLatestCheckpointsOnly()` method
  - Groups checkpoints by WIR number AND BoxActivityId
  - Returns only the latest version for each group
  - Sorts checkpoints by WIR number for consistent display
  - Stores total version count in checkpoint for display
- **Added**: Helper methods for version display
  - `getCheckpointVersionLabel()` - Returns version label (e.g., "v2", "v3")
  - `hasMultipleVersions()` - Checks if checkpoint has multiple versions
  - `getTotalVersionCount()` - Returns total number of versions
- **Modified**: `loadWIRCheckpoints()` method
  - Now filters checkpoints to show only the latest versions
  - Logs how many versions were found vs. displayed
- **Impact**: 
  - Stage Checkpoints tab in box details now shows only the latest checkpoint versions
  - When clicking "Review" or "Add Checklist Items", navigates to the latest version automatically
  - Users no longer see duplicate entries for rejected/resubmitted checkpoints

### 5. `box-details.component.html` & `.scss` - Version Badge UI
- **Added**: Version badge display in checkpoint header
  - Shows version label (e.g., "v2", "v3") next to checkpoint number
  - Only displays for versions > 1
  - Tooltip shows "Version X of Y" (e.g., "Version 3 of 4")
  - Blue badge with hover effect
- **Added**: "View All Versions" expandable section with full checkpoint cards
  - **Button location**: Positioned in the checkpoint card header (right side)
  - Button appears when checkpoint has multiple versions
  - Shows count: "View All (4)" or "Hide (4)" when expanded
  - Compact design to fit in header without cluttering
  - Expands to show all older versions as **full checkpoint cards**
  - Each older version displays **complete checkpoint card** with:
    - **Header**: Version badge (e.g., "v3", "v2", "v1") + Status badge (gray gradient styling)
    - **Body**: All checkpoint data
      - Title and description
      - Requested date and requestor
      - Inspector name and role
      - Checklist items count
      - Last update date
    - **Actions Footer**: 
      - Right-aligned layout with:
        - "Historical Version (Read-Only)" badge with lock icon (amber/yellow)
        - Print button
        - Download button
  - **Latest version** remains at top with full card and all actions
  - **Visual distinction**: 
    - Latest = Teal gradient header + full actions
    - Older = Gray gradient header + right-aligned read-only badge and limited actions (Print/Download)
- **Impact**: 
  - Users can see ALL checkpoint details for every version
  - Easy comparison between versions (same data layout)
  - Clear audit trail with full historical data
  - Prevents accidental editing with visual "Read-Only" indicator
  - Latest version maintains full functionality

## Testing Recommendations

1. **Test Scenario 1: Single Checkpoint**
   - Create a WIR with one checkpoint (v1)
   - Approve it
   - Verify position unlocks correctly

2. **Test Scenario 2: Multiple Checkpoints (Rejection → Approval)**
   - Create WIR-3 checkpoint (v1)
   - Reject it
   - Create new version (v2)
   - Approve v2
   - **Expected**: Position should unlock when trying to update progress for the next WIR
   - **Before fix**: Position remained locked
   - **After fix**: Position unlocks correctly

3. **Test Scenario 3: Multiple Rejections**
   - Create WIR-3 checkpoint (v1) → Reject
   - Create v2 → Reject
   - Create v3 → Approve
   - **Expected**: Position should unlock (checking v3, not v1 or v2)

4. **Test Scenario 4: Stage Checkpoints List**
   - Create WIR-3 checkpoint (v1) → Reject
   - Create v2 → Reject
   - Create v3 → Approve
   - Navigate to Box Details → Stage Checkpoints tab
   - **Expected**: 
     - Only ONE WIR-3 entry should be displayed (v3)
     - Status badge should show "Approved" (not "Rejected")
     - Click "Review" button → Should navigate to v3 checkpoint
   - **Before fix**: Showed 3 entries (v1, v2, v3) or showed v1's status
   - **After fix**: Shows only v3 with correct status

5. **Test Scenario 5: View All Versions Feature with Full Checkpoint Cards**
   - Create WIR-2 checkpoint (v1) → Reject
   - Create v2 → Reject
   - Create v3 → Approve
   - Navigate to Box Details → Stage Checkpoints tab
   - **Expected**:
     - See "WIR-2 [v3]" with "Approved" status (teal gradient header)
     - See "[▶ View All (3)]" button in the header (right side, after status badge)
     - Click "[▶ View All (3)]" button in the header
     - Should expand to show **2 full checkpoint cards** (v2 and v1):
       - **v2 Card**: 
         - Gray gradient header with "Stage-2 [v2]" and "Rejected" status
         - Full body with requested date, inspector, checklist count, last update
         - Footer (right-aligned): "🔒 Historical Version (Read-Only)" badge + [Print] [Download]
       - **v1 Card**:
         - Gray gradient header with "Stage-2 [v1]" and "Rejected" status
         - Full body with all checkpoint information
         - Footer (right-aligned): "🔒 Historical Version (Read-Only)" badge + [Print] [Download]
     - v3 (latest) at top should still show:
       - Teal gradient header
       - Full actions: [Add Checklist] [Review] [Print] [Download]
     - Click Print on v2 → Should print v2 (not v3)
     - Click Download on v1 → Should download v1 specifically
     - Verify all v2 and v1 data is visible (dates, inspector, checklist items)
     - Click "Hide Versions" → Should collapse and only show v3
   - **Impact**: 
     - Users can see complete historical data for audit/reference
     - Easy to compare versions side-by-side
     - Clear visual indicators prevent editing old versions

## Navigation Flow (Automatic Latest Version)

When a user clicks "Review" or "Add Checklist Items" from the Stage Checkpoints list:

1. **Box Details Page** → Displays only latest checkpoint versions (filtered by `getLatestCheckpointsOnly()`)
2. **Navigation** → Routes to `/projects/{projectId}/boxes/{boxId}/activities/{boxActivityId}/qa-qc`
3. **QA/QC Page** → Calls `getWIRCheckpointByActivity()` which returns the latest version
4. **Result** → User always works with the latest checkpoint version

This flow ensures users never accidentally view or edit old rejected versions.

## Impact

This fix ensures that:
- ✅ Position lock status is determined by the LATEST checkpoint version only
- ✅ Users can proceed after a rejected WIR is resubmitted and approved
- ✅ The workflow continues smoothly after rejection → resubmission → approval cycles
- ✅ Historical rejected versions don't block progress
- ✅ **WIR status displayed in activity table shows the correct status of the latest checkpoint version**

## Additional Bug Fixed

While reviewing the code, we discovered that the `activity-table.component.ts` had the checkpoints sorted correctly but was returning the WRONG element from the array:

**The Bug:**
```typescript
// Line 931 - BEFORE (Incorrect)
return versions && versions.length > 0 ? versions[versions.length - 1] : null;
```

This was returning the **last** element (oldest version) instead of the **first** element (latest version).

**The Fix:**
```typescript
// Line 931 - AFTER (Correct)
return versions && versions.length > 0 ? versions[0] : null;
```

Since the array is sorted in **descending order** (newest first), `versions[0]` is the latest version.

**Impact:**
- The WIR status badge in the activity table was showing the status of the **oldest** checkpoint version
- This caused confusion when a WIR had multiple versions
- Now it correctly shows the status of the **latest** checkpoint version
