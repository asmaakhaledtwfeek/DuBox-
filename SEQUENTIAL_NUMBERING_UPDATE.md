# Sequential Numbering Update

## Overview
Updated the Activity Checklist Review modal to display sequential numbering (1, 2, 3...) within each section, instead of using the original sequence numbers from the database.

## Problem Statement

**Before:**
Items displayed with their original database sequence numbers, which could be non-sequential:
- Item #7: Ensure method statement...
- Item #11: Ensure materials are stored...
- Item #17: Check the expiry date...

This made it confusing for users to track progress and reference items.

**After:**
Items are re-numbered sequentially within each section:
- Item #1: Ensure method statement...
- Item #2: Ensure materials are stored...
- Item #3: Check the expiry date...

## Implementation

### Changes Made

1. **New Interface** (`ActivityCheckListItemWithReviewAndDisplaySeq`):
   ```typescript
   interface ActivityCheckListItemWithReviewAndDisplaySeq extends ActivityCheckListItemWithReview {
     displaySequence?: number;
   }
   ```

2. **Updated `groupChecklistItems()` Method**:
   - Sorts items by original sequence (for correct ordering)
   - Assigns new `displaySequence` starting from 1 for each section
   - Each section gets its own sequential numbering

3. **Updated Template**:
   - Changed from `{{ item.sequence }}` to `{{ item.displaySequence }}`
   - Displays the new sequential number in the `#` column

### Logic Flow

```typescript
groupChecklistItems() {
  // ... grouping logic ...
  
  checklist.sections.forEach(section => {
    // 1. Sort items by original sequence
    section.items.sort((a, b) => a.sequence - b.sequence);
    
    // 2. Assign new display sequence (1, 2, 3...)
    section.items.forEach((item, index) => {
      item.displaySequence = index + 1;
    });
  });
}
```

## Examples

### Example 1: GENERAL Section
**Original Sequences:** 7, 11, 17  
**Display Sequences:** 1, 2, 3

```
GENERAL
  1. Ensure method statement, materials and shop drawings are approved
  2. Ensure materials are stored under dry, clean, shaded area
  3. Check the expiry date of the material prior to applications
```

### Example 2: PREPARATION & SETTING OUT Section
**Original Sequences:** 8, 18  
**Display Sequences:** 1, 2 (restarts)

```
PREPARATION & SETTING OUT
  1. Ensure Drawing Stamp, Signature, Element Tags are correct
  2. Floor Setting Out/Layout as per drawing
```

### Example 3: Multiple Sections
```
┌─────────────────────────────────────┐
│ CHK-PC-001 Construction...         │
│                                     │
│ GENERAL                            │
│  #1 - Item A (original seq: 7)    │
│  #2 - Item B (original seq: 11)   │
│  #3 - Item C (original seq: 17)   │
│                                     │
│ PREPARATION & SETTING OUT          │
│  #1 - Item D (original seq: 8)    │ ← Restarts at 1
│  #2 - Item E (original seq: 18)   │
│                                     │
│ ERECTION / ASSEMBLING              │
│  #1 - Item F (original seq: 6)    │ ← Restarts at 1
│  #2 - Item G (original seq: 15)   │
└─────────────────────────────────────┘
```

## Benefits

1. **Better Readability**
   - Sequential numbers are easier to follow (1, 2, 3 vs 7, 11, 17)
   - Users can quickly count items in a section

2. **Clearer Progress Tracking**
   - "I've reviewed 3 out of 5 items in this section"
   - Easier to reference specific items in discussions

3. **Professional Appearance**
   - Clean, organized numbering
   - Matches user expectations for checklists

4. **Maintains Data Integrity**
   - Original sequence numbers are preserved for sorting
   - Backend data remains unchanged
   - Only the display is modified

## Technical Details

### Data Flow
```
Backend → Original Sequence (7, 11, 17, 8, 18...)
    ↓
Grouping → Sort by original sequence within section
    ↓
Display → New sequence (1, 2, 3, 1, 2...)
```

### Properties
- **`sequence`**: Original sequence from database (preserved)
- **`displaySequence`**: New sequential number (computed)
- **Display**: Uses `displaySequence` in UI
- **Sorting**: Uses `sequence` for ordering

## Impact

### User Experience
- ✅ Easier to read and follow
- ✅ Clear progress indication
- ✅ Consistent numbering within sections
- ✅ Professional presentation

### Backend
- ✅ No changes required
- ✅ Original sequence preserved
- ✅ Sorting logic unchanged

### Frontend
- ✅ Minimal code changes
- ✅ Computed during grouping
- ✅ No performance impact

## Testing

### Verification Steps
1. ✅ Items within a section show 1, 2, 3...
2. ✅ Each section restarts numbering at 1
3. ✅ Items appear in correct order (by original sequence)
4. ✅ All sections across multiple checklists work correctly
5. ✅ Expand/collapse doesn't affect numbering
6. ✅ Review submission includes all items

### Test Cases
```
Test 1: Single Section
  Original: [7, 11, 17]
  Expected: [1, 2, 3] ✓

Test 2: Multiple Sections
  Section A Original: [7, 11, 17]
  Section A Expected: [1, 2, 3] ✓
  Section B Original: [8, 18]
  Section B Expected: [1, 2] ✓

Test 3: Non-Sequential Original
  Original: [17, 3, 25, 8]
  Sorted: [3, 8, 17, 25]
  Expected: [1, 2, 3, 4] ✓
```

## Backward Compatibility

- ✅ No API changes
- ✅ No database changes
- ✅ Display-only modification
- ✅ Existing functionality preserved
