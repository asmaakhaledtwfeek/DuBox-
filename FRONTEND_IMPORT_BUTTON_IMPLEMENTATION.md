# Frontend: Import Excel Button Implementation

## Summary
Added an **Import Excel** button next to the existing **Export Excel** button for each box type in the Project Materials Management page. Users can now upload edited Excel files to update material quantities.

---

## Changes Made

### 1. Service Layer (`box-type-material.service.ts`)

Added two new methods:

#### **Export Method**
```typescript
exportBoxTypeMaterialsToExcel(projectBoxTypeId: number): Observable<Blob>
```
- Calls backend endpoint: `GET /api/box-type-materials/box-type/{projectBoxTypeId}/export`
- Returns Excel file as blob for download

#### **Import Method**
```typescript
importBoxTypeMaterialsFromExcel(projectBoxTypeId: number, file: File): Observable<ImportResult>
```
- Calls backend endpoint: `POST /api/box-type-materials/box-type/{projectBoxTypeId}/import`
- Uploads Excel file using FormData
- Returns detailed result with:
  - `successCount`: Number of successfully updated materials
  - `failureCount`: Number of failed updates
  - `errors[]`: Array of error messages (row-level)
  - `warnings[]`: Array of warning messages

---

### 2. Component Logic (`box-type-materials.component.ts`)

Added two new methods:

#### **triggerImportExcel(boxTypeName: string)**
- Creates a hidden file input element
- Accepts `.xlsx` and `.xls` files only
- Triggers file selection dialog
- Calls `importBoxTypeMaterials()` when file is selected

#### **importBoxTypeMaterials(projectBoxTypeId, boxTypeName, file)**
- Uploads the selected file to the backend
- Shows loading state during upload
- Displays detailed results:
  - ✅ Success message if materials were updated
  - ⚠️ Error messages (shows first 5, indicates if more exist)
  - 📝 Warnings logged to console
- Automatically reloads materials after successful import
- Auto-dismisses messages after 8 seconds

---

### 3. Template UI (`box-type-materials.component.html`)

Added Import Excel button next to Export button:

**Location:** Line 229-238 (after Export Excel button)

```html
<button class="btn btn-primary btn-sm" 
        (click)="triggerImportExcel(boxTypeName)" 
        [disabled]="loading"
        title="Import Excel file to update materials for this box type">
  <svg><!-- Upload icon --></svg>
  Import Excel
</button>
```

**Features:**
- Blue button (primary style) to distinguish from green Export button
- Upload icon (arrow pointing up) vs Export icon (arrow pointing down)
- Disabled during loading states
- Tooltip explaining functionality
- Positioned immediately after Export button

---

## User Workflow

### Step 1: Export Excel
1. User clicks **Export Excel** button for a box type (e.g., "S1")
2. Downloads file: `S1_Materials_2026-02-08.xlsx`

### Step 2: Edit Excel
1. User opens the Excel file
2. Edits only the highlighted columns:
   - **Quantity Per Box**
   - **Delivered Quantity**
   - **Notes**
3. Saves the file

### Step 3: Import Excel
1. User clicks **Import Excel** button for the same box type
2. Selects the edited Excel file
3. System uploads and processes the file
4. Shows results:
   - "Import completed. 12 material(s) updated successfully."
   - OR errors like: "Row 5: Delivered Quantity (100) cannot exceed Total Quantity (80)"

### Step 4: View Updates
- Materials list automatically refreshes
- Updated quantities and notes are visible immediately

---

## UI Layout

**Before (only Export button):**
```
┌─────────────────────────────────────────────────┐
│ 📦 S1 (15 materials)    [Export Excel ↓]       │
└─────────────────────────────────────────────────┘
```

**After (Export + Import buttons):**
```
┌─────────────────────────────────────────────────────────────┐
│ 📦 S1 (15 materials)    [Export Excel ↓]  [Import Excel ↑] │
└─────────────────────────────────────────────────────────────┘
```

---

## Error Handling

### File Validation
- ✅ Only `.xlsx` and `.xls` files accepted
- ✅ File size limit: 10 MB (server-side)

### Import Errors (shown to user)
```
Import completed with errors:
Row 3: Material Code is required
Row 5: Delivered Quantity (100) cannot exceed Total Quantity (80)
Row 7: Material with code 'MAT-999' not found in the system
... and 3 more errors
```

### Success Messages
```
Import completed. 12 material(s) updated successfully.
```

### Warnings (logged to console)
```
Row 3: Total Quantity in Excel (150) differs from calculated value (160)
```

---

## Technical Details

### File Upload
- Uses `FormData` for multipart file upload
- Sends file as `file` parameter
- Content-Type: `multipart/form-data`

### Response Format
```typescript
{
  message: string;           // Overall message
  successCount: number;      // Number of successful updates
  failureCount: number;      // Number of failed updates
  errors: string[];          // Array of error messages
  warnings: string[];        // Array of warnings
}
```

### Loading States
- Button disabled during:
  - File upload
  - Excel processing
  - Material reloading
- Visual loading indicator shown

---

## Button Styles

### Export Button
- **Color:** Green (`btn-success`)
- **Icon:** Download arrow (pointing down)
- **Purpose:** Download current data

### Import Button
- **Color:** Blue (`btn-primary`)
- **Icon:** Upload arrow (pointing up)
- **Purpose:** Upload edited data

---

## Testing Checklist

- [x] ✅ Service methods added
- [x] ✅ Component methods implemented
- [x] ✅ Import button added to UI
- [x] ✅ Button positioned next to Export
- [x] ✅ File input accepts only Excel files
- [x] ✅ Loading state handled
- [x] ✅ Success messages displayed
- [x] ✅ Error messages displayed
- [x] ✅ Materials auto-reload after import
- [x] ✅ No linter errors

---

## Files Modified

1. **Service:** `dubox-frontend/src/app/core/services/box-type-material.service.ts`
   - Added: `exportBoxTypeMaterialsToExcel()`
   - Added: `importBoxTypeMaterialsFromExcel()`

2. **Component:** `dubox-frontend/src/app/features/projects/box-type-materials/box-type-materials.component.ts`
   - Added: `triggerImportExcel()`
   - Added: `importBoxTypeMaterials()`

3. **Template:** `dubox-frontend/src/app/features/projects/box-type-materials/box-type-materials.component.html`
   - Added: Import Excel button (line 229-238)

---

## Next Steps (Optional Enhancements)

1. **Progress Indicator**: Show upload progress percentage
2. **Drag & Drop**: Allow dragging Excel files onto the page
3. **Preview**: Show a preview of changes before applying
4. **Batch Import**: Import for multiple box types at once
5. **History**: Keep track of import history with timestamps

---

**Implementation Date:** February 8, 2026  
**Status:** ✅ Complete
