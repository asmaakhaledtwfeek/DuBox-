# Checklist Items Moved to Assembly & Joints Activity

## Summary
Successfully moved 6 checklist items from "PODS installation" activity to "Assembly & joints" activity as requested.

## Changes Made

### 1. Items Added to "Assembly & joints" (Activity ID: 10000001-0000-0000-0000-000000000001)

The following items were added with sequences 15-20:

| Seq | Item Description | Predefined Item ID | Status |
|-----|-----------------|-------------------|---------|
| 15  | Level Line to be marked at walls on 1000mm from FFL. | 5a6fcff3-aacd-46dc-b84b-d3369e3b5e3c | ✓ Added |
| 16  | Erection & Connections of Floor Slab | 2643e140-a1d0-4ed8-9e93-35bf3f62e63e | ✓ Added |
| 17  | Bottom slab shall read 1020mm at 1000m FFL line or as per drawing | d687bd92-0b37-452c-96ce-c0f3be44f178 | ✓ Added |
| 18  | Erection of partition walls by Temporary Support | 29b5036f-51ca-475c-aa6d-4de31094bfe3 | ✓ Added |
| 19  | Panel to Panel Connections are as per drawing | a74d7b03-d1d6-432b-abb3-1871d8a1c305 | ✓ Added |
| 20  | Dimensions (outer, inner and diagonal), Line and Level Grouting | ecd6ec00-531f-4d16-a7fd-381e466a6f38 | ✓ Added |

### 2. Items Removed from "PODS installation" (Activity ID: 10000001-0000-0000-0000-000000000002)

The following 6 items were removed from PODS installation:
- Level Line to be marked at walls on 1000mm from FFL.
- Erection & Connections of Floor Slab
- Bottom slab shall read 1020mm at 1000m FFL line or as per drawing
- Erection of partition walls by Temporary Support
- Panel to Panel Connections are as per drawing
- Dimensions (outer, inner and diagonal), Line and Level Grouting

The remaining items in PODS installation were re-sequenced starting from sequence 1.

## Files Modified

### ActivityCheckListItemLinksData.cs
- **Path**: `Dubox.Infrastructure/Seeding/ActivityCheckListItemLinksData.cs`
- **Changes**:
  - Added 6 new `ActivityCheckListItem` entries for "Assembly & joints" activity (sequences 15-20)
  - Removed 6 `ActivityCheckListItem` entries from "PODS installation" activity
  - Re-sequenced remaining PODS installation items

## Verification Status

✓ All items exist in `PredefinedChecklistItemsData.cs` (no new items needed to be created)
✓ Items added to "Assembly & joints" with correct sequences (15-20)
✓ Items removed from "PODS installation"
✓ Remaining PODS installation items re-sequenced correctly

## Next Steps

1. **Build the solution** to ensure no compilation errors
2. **Run database migrations** to apply the seeding changes
3. **Test the changes** by verifying:
   - "Assembly & joints" activity displays all 20 checklist items
   - "PODS installation" activity no longer shows the moved items
   - All sequences are correct and items appear in the right order

## Notes

- The items were already present in the predefined checklist items database, so no new items were created
- New Activity Check List Item IDs were generated using the pattern: `a1001001-0001-0001-0001-0000000000XX`
- Box Closure activity (ID: 10000001-0000-0000-0000-000000000004) was not modified as it was not mentioned in the request
