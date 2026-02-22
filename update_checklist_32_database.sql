-- Script to update Checklist 32 in the database
-- Run this script directly against your Dubox database

BEGIN TRANSACTION;

-- Step 1: Delete ActivityCheckListItem records that reference checklist 32 items
PRINT 'Deleting ActivityCheckListItem records referencing Checklist 32...';
DELETE FROM ActivityCheckListItems 
WHERE PredefinedChecklistItemId IN (
    SELECT PredefinedItemId 
    FROM PredefinedChecklistItems 
    WHERE ChecklistNumber = 32
);

PRINT 'Deleted ' + CAST(@@ROWCOUNT AS VARCHAR) + ' ActivityCheckListItem records.';

-- Step 2: Delete old predefined checklist items for checklist 32
PRINT 'Deleting old Predefined Checklist Items for Checklist 32...';
DELETE FROM PredefinedChecklistItems 
WHERE ChecklistSectionId IN (
    SELECT ChecklistSectionId 
    FROM ChecklistSections 
    WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168'
);

PRINT 'Deleted ' + CAST(@@ROWCOUNT AS VARCHAR) + ' PredefinedChecklistItems.';

-- Step 3: Delete old sections for checklist 32
PRINT 'Deleting old Checklist Sections for Checklist 32...';
DELETE FROM ChecklistSections 
WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168';

PRINT 'Deleted ' + CAST(@@ROWCOUNT AS VARCHAR) + ' ChecklistSections.';

-- Step 4: Update checklist 32 name
PRINT 'Updating Checklist 32 name...';
UPDATE Checklists 
SET Name = 'Check List for Pre-Loading of Completed Precast Modular (MEP & Electrical)'
WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168';

PRINT 'Updated Checklist 32 name.';

COMMIT TRANSACTION;

PRINT '';
PRINT '✅ Checklist 32 old data successfully deleted!';
PRINT '';
PRINT 'Next steps:';
PRINT '1. Create a new migration: dotnet ef migrations add InsertNewChecklist32Data';
PRINT '2. Apply the migration: dotnet ef database update';
PRINT '3. The new checklist 32 data will be automatically seeded.';
