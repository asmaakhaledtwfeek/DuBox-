# Fix: Excel Import Row Number Mismatch

## Problem
User uploaded Excel file with 16 data rows (rows 2-17 in Excel), but the system reported an error on "Row 17: Material Code is required" even though row 17 had data.

## Root Cause
The Excel parsing was including **empty rows** after the actual data. When Excel files have formatting or previously deleted rows, the parser can pick up these empty rows as data rows.

## Analysis
Looking at the Excel structure:
- Row 1: Column headers (Status, Material Code, Material Name, etc.)
- Rows 2-17: 16 actual data rows
- Row 18 (or beyond): Potentially empty rows picked up by the parser

The error "Row 17: Material Code is required" suggests an empty row was being processed.

## Solution Implemented

Added filtering logic to **skip empty rows** before processing:

```csharp
// Filter out completely empty rows (rows where all key fields are empty)
materialRows = materialRows
    .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode) || 
               !string.IsNullOrWhiteSpace(r.MaterialName) || 
               r.QuantityPerBox > 0)
    .ToList();
```

This filters out rows where:
- Material Code is empty/null/whitespace **AND**
- Material Name is empty/null/whitespace **AND**
- Quantity Per Box is 0 or not set

## Row Numbering
The row number calculation remains:
```csharp
var rowNumber = i + 2; // Excel rows start at 1, plus 1 for header row
```

This is correct because:
- Excel rows are 1-indexed
- Row 1 is the header
- Data starts at row 2
- So for array index `i=0`, the Excel row is `0 + 2 = 2` ✅

## Benefits
1. ✅ **No false errors** for empty rows
2. ✅ **Accurate row numbers** in error messages
3. ✅ **Handles trailing empty rows** gracefully
4. ✅ **Processes only valid data** rows
5. ✅ **Better user experience** - only reports errors for actual data rows

## Testing Scenarios
- [ ] Excel file with exactly matching rows (no empty rows)
- [ ] Excel file with trailing empty rows
- [ ] Excel file with empty rows in the middle
- [ ] Excel file with only formatted but empty cells
- [ ] Large Excel file with many empty trailing rows

## Files Modified
- `ImportBoxTypeMaterialsFromExcelCommandHandler.cs` - Added empty row filtering logic

## Expected Behavior Now
- Empty rows are silently skipped
- Only rows with actual data (Material Code, Material Name, or Quantity Per Box) are processed
- Error messages show accurate Excel row numbers for problematic data
- No false "Material Code is required" errors for empty rows
