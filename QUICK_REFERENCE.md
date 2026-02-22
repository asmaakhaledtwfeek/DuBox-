# Box Type Material Import/Export - Quick Reference

## 🎯 Purpose
Import and export box type materials at the **box type level** (not project level) via Excel files. Users can edit `Delivered Quantity` and `Quantity Per Box` values.

---

## 📋 Key Validation Rule
**⚠️ IMPORTANT: `Delivered Quantity` CANNOT exceed `Total Quantity`**

Formula: `Total Quantity = Number of Boxes × Quantity Per Box`

---

## 🔌 API Endpoints

### 1. Export Materials
```
GET /api/box-type-materials/box-type/{projectBoxTypeId}/export
```
Downloads an Excel file with current box type materials.

### 2. Import Materials
```
POST /api/box-type-materials/box-type/{projectBoxTypeId}/import
Content-Type: multipart/form-data
Parameter: file (Excel file)
```
Updates materials from edited Excel file.

---

## 📝 Excel File Structure

### Editable Columns (highlighted in light blue):
- ✅ **Quantity Per Box** - Must be > 0
- ✅ **Delivered Quantity** - Must be ≤ Total Quantity
- ✅ **Notes** - Optional text

### Read-Only Columns:
- ❌ Status
- ❌ Material Code
- ❌ Material Name
- ❌ Category
- ❌ Total Quantity (auto-calculated)
- ❌ Required Before (Days)

---

## ✅ Validation Rules

| Rule | Description | Error if Failed |
|------|-------------|----------------|
| Material Code exists | Material must be in system | "Material with code 'X' not found" |
| Material assigned | Material must be assigned to box type | "Box type material assignment not found" |
| Quantity Per Box > 0 | Must be positive integer | "Quantity Per Box must be greater than 0" |
| Delivered ≤ Total | Cannot over-deliver | "Delivered Quantity (X) cannot exceed Total Quantity (Y)" |

---

## 🎨 Excel Color Coding

| Color | Meaning |
|-------|---------|
| 🟦 Light Blue | Editable column |
| 🟢 Light Green | Status = DELIVERED |
| 🟡 Light Yellow | Status = PENDING |
| 🟦 Blue Header | Column header |
| 🟨 Yellow Banner | Instructions row |

---

## 📊 Import Response Example

```json
{
  "message": "Import completed. 12 material(s) updated successfully.",
  "successCount": 12,
  "failureCount": 1,
  "errors": [
    "Row 5: Delivered Quantity (100) cannot exceed Total Quantity (80)"
  ],
  "warnings": [
    "Row 3: Total Quantity in Excel differs from calculated value"
  ]
}
```

---

## 🔄 Workflow

```
1. Export Excel → Edit → Import → Review Results
   ↓            ↓       ↓         ↓
   Download    Modify   Upload    Check Errors
   .xlsx       values   file      & Warnings
```

---

## 💡 Tips

1. **Always export first** to get the latest data
2. **Check Total Quantity** before setting Delivered Quantity
3. **Review warnings** - they don't block import but may indicate issues
4. **Material Code** is the unique identifier - don't change it
5. **Partial success** is allowed - valid rows update even if some fail

---

## 🚨 Common Errors

| Error Message | Cause | Solution |
|--------------|-------|----------|
| "No file uploaded" | Missing file in request | Include file in form data |
| "Invalid file format" | Wrong file type | Use .xlsx or .xls only |
| "Delivered Quantity cannot exceed Total Quantity" | Over-delivery | Reduce Delivered Quantity or increase Quantity Per Box |
| "Material not found" | Wrong Material Code | Use Material Code from export |
| "Box type material assignment not found" | Material not assigned | Assign material to box type first |

---

## 📁 Files Created

### Backend:
- `ImportBoxTypeMaterialsFromExcelCommand.cs`
- `ImportBoxTypeMaterialsFromExcelCommandHandler.cs`
- `GenerateBoxTypeMaterialsExcelQuery.cs`
- `GenerateBoxTypeMaterialsExcelQueryHandler.cs`
- `BoxTypeMaterialsController.cs` (updated)
- `BoxTypeMaterialDto.cs` (updated)

### Documentation:
- `IMPLEMENTATION_BOX_TYPE_MATERIAL_IMPORT.md`
- `API_USAGE_EXAMPLES.md`
- `QUICK_REFERENCE.md` (this file)

---

## 🧪 Testing Checklist

- [ ] Export Excel with valid box type ID
- [ ] Import with all valid data
- [ ] Import with DeliveredQuantity > TotalQuantity (should fail)
- [ ] Import with QuantityPerBox = 0 (should fail)
- [ ] Import with non-existent Material Code (should fail)
- [ ] Import with material not assigned to box type (should fail)
- [ ] Verify Total Quantity auto-calculation
- [ ] Verify Delivery Progress updates
- [ ] Verify Status changes to "Arrived" at 100%
- [ ] Test partial success scenario

---

## 📞 Support

For issues or questions:
1. Check error messages in import response
2. Review API_USAGE_EXAMPLES.md for integration examples
3. Consult IMPLEMENTATION_BOX_TYPE_MATERIAL_IMPORT.md for technical details

---

**Last Updated**: February 8, 2026
**Version**: 1.0
