# Script to create a manual migration for updating checklist 32

$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$migrationName = "${timestamp}_UpdateChecklist32Data"
$migrationPath = ".\Dubox.Infrastructure\Migrations\${migrationName}.cs"

$migrationContent = @"
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dubox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChecklist32Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Delete ActivityCheckListItem records that reference checklist 32 items
            migrationBuilder.Sql(@""
                DELETE FROM ActivityCheckListItems 
                WHERE PredefinedChecklistItemId IN (
                    SELECT PredefinedItemId 
                    FROM PredefinedChecklistItems 
                    WHERE ChecklistNumber = 32
                );
            "");

            // Step 2: Delete old checklist 32 sections and items
            migrationBuilder.Sql(@""
                -- Delete predefined checklist items for checklist 32
                DELETE FROM PredefinedChecklistItems 
                WHERE ChecklistSectionId IN (
                    SELECT ChecklistSectionId 
                    FROM ChecklistSections 
                    WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168'
                );
                
                -- Delete sections for checklist 32
                DELETE FROM ChecklistSections 
                WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168';
                
                -- Update checklist 32 name
                UPDATE Checklists 
                SET Name = 'Check List for Pre-Loading of Completed Precast Modular (MEP & Electrical)'
                WHERE ChecklistId = '98448459-61c2-4ab7-9e77-94cd67601168';
            "");

            // Note: The new checklist 32 data will be inserted automatically through the 
            // ChecklistSeedDataFromJson seeding when the application starts or when you
            // run the next migration. The seeding logic will detect the missing data and insert it.
            
            // Step 3: Force re-seeding by updating the context
            // This will be handled by the seeding logic in ChecklistSeedDataFromJson
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Warning: Down migration not fully implemented
            // Reversing this migration would require restoring the original checklist 32 data
            throw new NotSupportedException("This migration cannot be reversed automatically. Please restore from backup if needed.");
        }
    }
}
"@

$migrationContent | Out-File -FilePath $migrationPath -Encoding UTF8
Write-Host "✅ Manual migration created: $migrationPath"
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Review the migration file"
Write-Host "2. Run: dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api"
Write-Host "3. The new checklist 32 data will be seeded automatically"
