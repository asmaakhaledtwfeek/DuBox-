# Box Type Material Excel Import/Export Feature

## Overview
This implementation adds the ability to import and export box type materials via Excel files at the **box type level** (not project level). Users can edit `DeliveredQuantity` and `QuantityPerBox` in the Excel file and re-import to update the system.

## Key Features

### 1. **Export Box Type Materials to Excel**
- **Endpoint**: `GET /api/box-type-materials/box-type/{projectBoxTypeId}/export`
- **Purpose**: Generate an Excel file with all materials assigned to a specific box type
- **File Contents**:
  - Status (DELIVERED/PENDING)
  - Material Code
  - Material Name
  - Category
  - **Quantity Per Box** (editable)
  - **Delivered Quantity** (editable)
  - Total Quantity (calculated: BoxCount × QuantityPerBox)
  - Required Before (Days)
  - Notes (editable)

### 2. **Import Box Type Materials from Excel**
- **Endpoint**: `POST /api/box-type-materials/box-type/{projectBoxTypeId}/import`
- **Purpose**: Update box type materials from an edited Excel file
- **Validations**:
  - ✅ Material Code must exist in the system
  - ✅ Material must be assigned to the box type
  - ✅ QuantityPerBox must be greater than 0
  - ✅ **DeliveredQuantity CANNOT exceed TotalQuantity** (key requirement)
  - ✅ Warnings if Excel TotalQuantity differs from calculated value

### 3. **Automatic Calculations**
- **Total Quantity** = Number of Boxes × Quantity Per Box
- **Delivery Progress** = (Delivered Quantity / Total Quantity) × 100%
- **Arrival Status**: Automatically set to "Arrived" when progress reaches 100%

## Files Created

### Commands
1. **`ImportBoxTypeMaterialsFromExcelCommand.cs`**
   - Command definition for import operation
   - Includes DTOs: `BoxTypeMaterialImportResultDto`, `ImportBoxTypeMaterialRowDto`

2. **`ImportBoxTypeMaterialsFromExcelCommandHandler.cs`**
   - Handles Excel file processing
   - Validates data and updates BoxTypeMaterial records
   - Returns success/failure counts with detailed error messages

### Queries
1. **`GenerateBoxTypeMaterialsExcelQuery.cs`**
   - Query definition for Excel export

2. **`GenerateBoxTypeMaterialsExcelQueryHandler.cs`**
   - Generates Excel file with current box type material data
   - Highlights editable columns in light blue
   - Color-codes status (green for DELIVERED, yellow for PENDING)
   - Includes instructions and freeze panes for better UX

### Controller Updates
**`BoxTypeMaterialsController.cs`** - Added two new endpoints:
- `GET box-type/{projectBoxTypeId}/export` - Download Excel template with data
- `POST box-type/{projectBoxTypeId}/import` - Upload edited Excel file

### DTO Updates
**`BoxTypeMaterialDto.cs`** - Added calculated field:
- `TotalQuantity` - Computed as `BoxCount × QuantityPerBox`

## Validation Rules

### During Import:
1. **Required Fields**:
   - Material Code
   
2. **Business Rules**:
   - Material must exist in the system
   - Material must be assigned to the specified box type
   - QuantityPerBox > 0
   - **DeliveredQuantity ≤ TotalQuantity** (enforced!)

3. **Warnings** (non-blocking):
   - Total Quantity mismatch between Excel and calculated value

## Excel File Structure

### Headers (Required):
| Column | Description | Editable | Validation |
|--------|-------------|----------|------------|
| Status | DELIVERED/PENDING | ❌ | Read-only |
| Material Code | Unique material identifier | ❌ | Must exist |
| Material Name | Name of material | ❌ | Read-only |
| Category | Material category | ❌ | Read-only |
| **Quantity Per Box** | Items needed per box | ✅ | Must be > 0 |
| **Delivered Quantity** | Items delivered | ✅ | Must be ≤ Total Quantity |
| Total Quantity | Calculated total needed | ❌ | Auto-calculated |
| Required Before (Days) | Lead time | ❌ | Read-only |
| Notes | Additional information | ✅ | Optional |

### Excel Features:
- ✅ Instruction row explaining usage
- ✅ Freeze panes for easy scrolling
- ✅ Color-coded editable columns (light blue)
- ✅ Status color-coding (green/yellow)
- ✅ Auto-fit columns for readability

## Usage Flow

### For End Users:

1. **Export Current Data**:
   ```
   GET /api/box-type-materials/box-type/{projectBoxTypeId}/export
   ```
   - Downloads `BoxTypeMaterials_BoxType{id}_{timestamp}.xlsx`

2. **Edit Excel File**:
   - Open in Excel/LibreOffice
   - Modify only the highlighted columns:
     - Quantity Per Box
     - Delivered Quantity
     - Notes
   - Save the file

3. **Re-Import Updated Data**:
   ```
   POST /api/box-type-materials/box-type/{projectBoxTypeId}/import
   Form Data: file = [Excel file]
   ```
   - System validates all changes
   - Returns detailed results with success/failure counts

4. **Review Results**:
   ```json
   {
     "message": "Import completed. 12 material(s) updated successfully.",
     "successCount": 12,
     "failureCount": 1,
     "errors": [
       "Row 5: Delivered Quantity (100) cannot exceed Total Quantity (80)"
     ],
     "warnings": [
       "Row 3: Total Quantity in Excel (150) differs from calculated value (160 = 8 boxes × 20 per box). Using calculated value."
     ]
   }
   ```

## API Endpoints Summary

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/box-type-materials/box-type/{id}/export` | Export materials to Excel |
| POST | `/api/box-type-materials/box-type/{id}/import` | Import updated materials from Excel |

## Error Handling

### Import Errors (Row-level):
- Missing required fields
- Material not found
- Material not assigned to box type
- Invalid quantity values
- **Delivered quantity exceeds total quantity**

### System Errors:
- Invalid file format
- Corrupt Excel file
- Missing headers
- Database errors

## Benefits

1. ✅ **Bulk Editing**: Update multiple materials at once
2. ✅ **Offline Capability**: Edit Excel offline, upload when ready
3. ✅ **Data Validation**: Comprehensive checks prevent invalid data
4. ✅ **Clear Feedback**: Detailed error messages for failed rows
5. ✅ **User-Friendly**: Color-coded, frozen headers, clear instructions
6. ✅ **Safe Updates**: Only materials assigned to the box type can be updated
7. ✅ **Automatic Calculations**: Total quantity and delivery progress auto-computed

## Technical Details

### Dependencies:
- EPPlus (Excel library) - already in project
- MediatR (CQRS pattern) - already in project
- Entity Framework Core - already in project

### Performance:
- Processes one row at a time
- Transaction support via UnitOfWork
- Partial success: Successful rows are saved even if some fail

### Security:
- Authorization required (inherited from controller)
- File size limit: 10 MB
- Only .xlsx and .xls files accepted
- Row-level validation prevents SQL injection

## Testing Recommendations

### Test Cases:
1. ✅ Export Excel with valid box type ID
2. ✅ Import with valid data
3. ✅ Import with DeliveredQuantity > TotalQuantity (should fail)
4. ✅ Import with missing Material Code (should fail)
5. ✅ Import with non-existent material (should fail)
6. ✅ Import with QuantityPerBox = 0 (should fail)
7. ✅ Import with partial failures (some rows succeed)
8. ✅ Verify automatic calculation of Total Quantity
9. ✅ Verify DeliveryProgress updates correctly
10. ✅ Verify IsArrived flag set when progress = 100%

## Future Enhancements (Optional)

- Add template download without data
- Support for bulk import across multiple box types
- Excel formula validation within the file
- Audit trail for import operations
- Email notifications on import completion
