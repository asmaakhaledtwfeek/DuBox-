# API Usage Examples

## Using the Box Type Material Excel Import/Export Endpoints

### Example 1: Export Box Type Materials

**Request:**
```http
GET /api/box-type-materials/box-type/123/export
Authorization: Bearer {your-token}
```

**Response:**
- Status: 200 OK
- Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
- File Download: `BoxTypeMaterials_BoxType123_20260208_143022.xlsx`

---

### Example 2: Import Box Type Materials (Success)

**Request:**
```http
POST /api/box-type-materials/box-type/123/import
Authorization: Bearer {your-token}
Content-Type: multipart/form-data

file: [Excel file uploaded]
```

**Sample Excel Data:**
| Status | Material Code | Material Name | Category | Quantity Per Box | Delivered Quantity | Total Quantity | Required Before (Days) | Notes |
|--------|--------------|---------------|----------|------------------|-------------------|----------------|----------------------|-------|
| PENDING | MAT-ELEC-011 | Cable Management System (Trays) | MEP - Electrical | 3 | 6 | 6 | 7 days | - |
| DELIVERED | MAT-ELEC-014 | Ceiling Fan | MEP - Electrical | 5 | 10 | 10 | 7 days | Delivered on time |

**Response:**
```json
{
  "message": "Import completed. 2 material(s) updated successfully.",
  "successCount": 2,
  "failureCount": 0,
  "errors": [],
  "warnings": []
}
```

---

### Example 3: Import with Validation Error

**Sample Excel Data (with error):**
| Status | Material Code | Material Name | Category | Quantity Per Box | Delivered Quantity | Total Quantity | Required Before (Days) | Notes |
|--------|--------------|---------------|----------|------------------|-------------------|----------------|----------------------|-------|
| PENDING | MAT-ELEC-011 | Cable Management System (Trays) | MEP - Electrical | 3 | **15** | 6 | 7 days | - |

Note: DeliveredQuantity (15) > TotalQuantity (6)

**Response:**
```json
{
  "message": "Import completed. 0 material(s) updated successfully.",
  "successCount": 0,
  "failureCount": 1,
  "errors": [
    "Row 3: Delivered Quantity (15) cannot exceed Total Quantity (6)"
  ],
  "warnings": []
}
```

---

### Example 4: Import with Partial Success

**Sample Excel Data:**
| Status | Material Code | Material Name | Category | Quantity Per Box | Delivered Quantity | Total Quantity | Required Before (Days) | Notes |
|--------|--------------|---------------|----------|------------------|-------------------|----------------|----------------------|-------|
| PENDING | MAT-ELEC-011 | Cable Management System (Trays) | MEP - Electrical | 3 | 6 | 6 | 7 days | OK |
| DELIVERED | MAT-ELEC-999 | Non-existent Material | MEP - Electrical | 5 | 10 | 10 | 7 days | Error |
| PENDING | MAT-ELEC-015 | Control Panel | MEP - Electrical | 5 | 8 | 10 | 7 days | OK |

**Response:**
```json
{
  "message": "Import completed. 2 material(s) updated successfully.",
  "successCount": 2,
  "failureCount": 1,
  "errors": [
    "Row 4: Material with code 'MAT-ELEC-999' not found in the system"
  ],
  "warnings": []
}
```

---

### Example 5: Import with Warning

**Sample Excel Data:**
| Status | Material Code | Material Name | Category | Quantity Per Box | Delivered Quantity | Total Quantity | Required Before (Days) | Notes |
|--------|--------------|---------------|----------|------------------|-------------------|----------------|----------------------|-------|
| PENDING | MAT-ELEC-011 | Cable Management System (Trays) | MEP - Electrical | 5 | 8 | **20** | 7 days | - |

Note: If there are actually only 2 boxes, TotalQuantity should be 10 (2 × 5), not 20

**Response:**
```json
{
  "message": "Import completed. 1 material(s) updated successfully.",
  "successCount": 1,
  "failureCount": 0,
  "errors": [],
  "warnings": [
    "Row 3: Total Quantity in Excel (20) differs from calculated value (10 = 2 boxes × 5 per box). Using calculated value."
  ]
}
```

---

## Frontend Integration Example (Angular/TypeScript)

### Service Method:

```typescript
// box-type-material.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BoxTypeMaterialService {
  private apiUrl = '/api/box-type-materials';

  constructor(private http: HttpClient) {}

  // Export box type materials to Excel
  exportBoxTypeMaterials(projectBoxTypeId: number): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/box-type/${projectBoxTypeId}/export`,
      { responseType: 'blob' }
    );
  }

  // Import box type materials from Excel
  importBoxTypeMaterials(projectBoxTypeId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post(
      `${this.apiUrl}/box-type/${projectBoxTypeId}/import`,
      formData
    );
  }
}
```

### Component Usage:

```typescript
// box-material-checklist.component.ts
export class BoxMaterialChecklistComponent {
  
  constructor(private boxTypeMaterialService: BoxTypeMaterialService) {}

  // Export to Excel
  onExport(): void {
    const projectBoxTypeId = 123; // Get from your context
    
    this.boxTypeMaterialService.exportBoxTypeMaterials(projectBoxTypeId)
      .subscribe({
        next: (blob) => {
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = `BoxTypeMaterials_${projectBoxTypeId}_${new Date().getTime()}.xlsx`;
          link.click();
          window.URL.revokeObjectURL(url);
        },
        error: (error) => {
          console.error('Export failed:', error);
          // Show error message to user
        }
      });
  }

  // Import from Excel
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (!file) return;

    const projectBoxTypeId = 123; // Get from your context
    
    this.boxTypeMaterialService.importBoxTypeMaterials(projectBoxTypeId, file)
      .subscribe({
        next: (response) => {
          console.log('Import successful:', response);
          
          // Show success message
          if (response.successCount > 0) {
            alert(`${response.successCount} material(s) updated successfully!`);
          }
          
          // Show errors if any
          if (response.errors && response.errors.length > 0) {
            const errorMsg = response.errors.join('\n');
            alert(`Errors:\n${errorMsg}`);
          }
          
          // Show warnings if any
          if (response.warnings && response.warnings.length > 0) {
            const warningMsg = response.warnings.join('\n');
            console.warn('Warnings:', warningMsg);
          }
          
          // Refresh the material list
          this.loadMaterials();
        },
        error: (error) => {
          console.error('Import failed:', error);
          alert('Import failed: ' + (error.error?.message || error.message));
        }
      });
  }

  private loadMaterials(): void {
    // Reload the materials list to show updated data
  }
}
```

### HTML Template:

```html
<!-- box-material-checklist.component.html -->
<div class="material-actions">
  <!-- Export Button -->
  <button 
    class="btn btn-primary" 
    (click)="onExport()">
    <i class="fas fa-download"></i>
    Export to Excel
  </button>

  <!-- Import Button -->
  <label class="btn btn-success" for="fileUpload">
    <i class="fas fa-upload"></i>
    Import from Excel
  </label>
  <input 
    type="file" 
    id="fileUpload" 
    accept=".xlsx,.xls" 
    (change)="onFileSelected($event)"
    style="display: none;">
</div>
```

---

## Postman/cURL Examples

### Export (cURL):
```bash
curl -X GET "https://your-api.com/api/box-type-materials/box-type/123/export" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -o "BoxTypeMaterials.xlsx"
```

### Import (cURL):
```bash
curl -X POST "https://your-api.com/api/box-type-materials/box-type/123/import" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@/path/to/your/file.xlsx"
```

---

## Common Error Codes

| Status Code | Error | Solution |
|------------|-------|----------|
| 400 | No file uploaded | Include file in form data |
| 400 | Invalid file format | Use .xlsx or .xls files only |
| 400 | Excel validation failed: Missing required column: X | Ensure all required headers are present |
| 400 | Box type with ID X not found | Verify the projectBoxTypeId exists |
| 401 | Unauthorized | Include valid Bearer token |
| 413 | File too large | File must be under 10 MB |

---

## Best Practices

1. **Always export before importing** to ensure you have the correct format and latest data
2. **Only edit highlighted columns** (light blue) in the Excel file
3. **Don't modify the header row** - it's used for validation
4. **Keep Material Code column unchanged** - it's the unique identifier
5. **Validate Delivered Quantity** doesn't exceed Total Quantity before importing
6. **Review warnings** - they don't block import but indicate potential issues
7. **Use descriptive Notes** to track changes and reasons
8. **Test with small batches first** before importing large files
