# Checklist Seeding Guide

## Overview

The checklist seeding system has been updated to load data from a JSON file with proper GUID identifiers. This allows for easier management and modification of checklist data without having to modify C# code.

## Files Created

### 1. JSON Data File
- **Location**: `Dubox.Infrastructure/Seeding/checklist_hierarchy_with_guids.json`
- **Purpose**: Contains all checklist, section, and item data with GUID identifiers
- **Content**: 33 checklists, 95 sections, and 698 predefined checklist items

### 2. Seeding Class
- **Location**: `Dubox.Infrastructure/Seeding/ChecklistSeedDataFromJson.cs`
- **Purpose**: Reads the JSON file and seeds the database using EF Core HasData
- **Features**:
  - Automatic JSON deserialization
  - GUID-based relationships
  - Error handling and logging
  - Maintains referential integrity

### 3. Configuration Files Updated
- **Dubox.Infrastructure.csproj**: Added configuration to copy JSON file to output directory
- **ApplicationDbContext.cs**: Updated to use new JSON-based seeding method

## Database Tables Seeded

The seeding populates three related tables:

1. **Checklists**
   - ChecklistId (GUID - Primary Key)
   - Name, Code, Discipline, SubDiscipline
   - PageNumber, WIRCode
   - ReferenceDocumentsJson, SignatureRolesJson
   - IsActive, CreatedDate

2. **ChecklistSections**
   - ChecklistSectionId (GUID - Primary Key)
   - ChecklistId (GUID - Foreign Key)
   - Title, Order
   - IsActive, CreatedDate

3. **PredefinedChecklistItems**
   - PredefinedItemId (GUID - Primary Key)
   - ChecklistSectionId (GUID - Foreign Key)
   - Description, Sequence
   - Reference, ChecklistNumber, Part
   - IsActive, CreatedDate

## How to Use

### Creating a Migration

To apply the new seeding data to your database, create and run a migration:

```bash
# Navigate to the solution directory
cd "c:\Users\asmaa.hassan\source\repos\Digital Engineering"

# Create a new migration
dotnet ef migrations add SeedChecklistsFromJson --project Dubox.Infrastructure --startup-project Dubox.Api

# Apply the migration to the database
dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api
```

### Verification Steps

After running the migration:

1. Check the database tables:
   ```sql
   SELECT COUNT(*) FROM Checklists;         -- Should return 33
   SELECT COUNT(*) FROM ChecklistSections;  -- Should return 95
   SELECT COUNT(*) FROM PredefinedChecklistItems; -- Should return 698
   ```

2. Verify relationships:
   ```sql
   -- Check checklist with sections
   SELECT c.Name, COUNT(cs.ChecklistSectionId) as SectionCount
   FROM Checklists c
   LEFT JOIN ChecklistSections cs ON c.ChecklistId = cs.ChecklistId
   GROUP BY c.ChecklistId, c.Name;

   -- Check sections with items
   SELECT cs.Title, COUNT(p.PredefinedItemId) as ItemCount
   FROM ChecklistSections cs
   LEFT JOIN PredefinedChecklistItems p ON cs.ChecklistSectionId = p.ChecklistSectionId
   GROUP BY cs.ChecklistSectionId, cs.Title;
   ```

## Modifying Checklist Data

### Option 1: Modify JSON File (Recommended)

1. Edit `Dubox.Infrastructure/Seeding/checklist_hierarchy_with_guids.json`
2. Maintain GUID format for all IDs
3. Ensure relationships are correct (ChecklistId references match)
4. Create a new migration to apply changes

### Option 2: Update Source JSON and Regenerate

1. Edit the original JSON file: `Documentation/checklist_hierarchy_fixed.json`
2. Run the conversion script:
   ```powershell
   cd "c:\Users\asmaa.hassan\source\repos\Digital Engineering"
   powershell -ExecutionPolicy Bypass -File Documentation/convert_ids_to_guid.ps1
   ```
3. Copy the new file:
   ```powershell
   Copy-Item "Documentation\checklist_hierarchy_with_guids.json" -Destination "Dubox.Infrastructure\Seeding\checklist_hierarchy_with_guids.json" -Force
   ```
4. Create a new migration

## JSON Structure Example

```json
{
  "checklists": [
    {
      "checklistId": "07df7a05-3f98-4cef-8603-2cf2a3438995",
      "name": "Construction of Precast Concrete Modular at Factory",
      "code": "CHK-PC-001",
      "discipline": "Civil",
      "subDiscipline": "Precast Concrete",
      "pageNumber": 1,
      "referenceDocumentsJson": "[\"DWG-CHK-PC-001\", \"SPEC-CHK-PC-001\"]",
      "signatureRolesJson": "[\"Site Engineer\", \"QC Inspector\"]",
      "wirCode": "WIR-PC-001",
      "isActive": true,
      "createdDate": "2025-02-10T00:00:00",
      "sections": [
        {
          "checklistSectionId": "825aaefe-e63e-4948-ae0b-4bf3203d5458",
          "title": "General",
          "order": 1,
          "checklistId": "07df7a05-3f98-4cef-8603-2cf2a3438995",
          "isActive": true,
          "createdDate": "2025-02-10T00:00:00",
          "items": [
            {
              "predefinedItemId": "955140b4-00aa-4ab2-a1f1-5ab51fc3f08d",
              "description": "Ensure method statement, materials and shop drawings are approved.",
              "sequence": 1,
              "isActive": true,
              "createdDate": "2025-02-10T00:00:00",
              "checklistSectionId": "825aaefe-e63e-4948-ae0b-4bf3203d5458",
              "reference": "REF-0001",
              "checklistNumber": 1,
              "part": 1
            }
          ]
        }
      ]
    }
  ]
}
```

## Troubleshooting

### Issue: JSON File Not Found
**Error**: "Warning: Checklist seed data file not found at: ..."

**Solution**: 
- Verify the JSON file exists in `Dubox.Infrastructure/Seeding/`
- Check the .csproj file has the `<CopyToOutputDirectory>` setting
- Rebuild the project

### Issue: Deserialization Errors
**Error**: JSON deserialization failures

**Solution**:
- Validate JSON syntax using a JSON validator
- Ensure all GUID strings are properly formatted
- Check that property names match the DTO classes

### Issue: Foreign Key Violations
**Error**: Foreign key constraint errors during seeding

**Solution**:
- Verify all `ChecklistId` references in sections exist in checklists
- Verify all `ChecklistSectionId` references in items exist in sections
- Ensure GUIDs are correctly copied between parent and child entities

### Issue: Duplicate Key Errors
**Error**: Duplicate primary key errors

**Solution**:
- Ensure all GUIDs are unique across the entire JSON file
- If updating existing data, remove old seed data first

## Migration Rollback

If you need to rollback the seeding:

```bash
# Rollback to previous migration
dotnet ef database update <PreviousMigrationName> --project Dubox.Infrastructure --startup-project Dubox.Api

# Remove the migration
dotnet ef migrations remove --project Dubox.Infrastructure --startup-project Dubox.Api
```

## Notes

- The old manual seeding methods (`ChecklistSeedData.SeedChecklists` and `PredefinedChecklistItemSeedData.SeedPredefinedChecklistItems`) have been commented out
- The JSON file is embedded in the build output, so deployment includes the seed data
- All dates are stored in UTC format
- The seeding only runs during migration creation, not on every application start

## Support

For issues or questions, refer to:
- Entity Framework Core documentation: https://docs.microsoft.com/ef/core/
- JSON validation: https://jsonlint.com/
