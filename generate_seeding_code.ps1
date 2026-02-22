# Script to generate C# seeding code for ActivityCheckListItems

$jsonPath = ".\Dubox.Infrastructure\Seeding\checklist_hierarchy_with_guids.json"
$json = Get-Content $jsonPath -Raw | ConvertFrom-Json

# Extract all predefined items
$allItems = @()
foreach ($checklist in $json.checklists) {
    foreach ($section in $checklist.sections) {
        foreach ($item in $section.items) {
            $allItems += [PSCustomObject]@{
                PredefinedItemId = $item.predefinedItemId
                Description = $item.description
                ChecklistNumber = $item.checklistNumber
                Part = $item.part
            }
        }
    }
}

# Define the mappings
$mappings = @(
    @{ActivityMasterId = "10000001-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 2}; Name = "ActivityMaster3"; GuidPrefix = "F3A"},
    @{ActivityMasterId = "10000001-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 1 -or $i.ChecklistNumber -eq 3}; Name = "ActivityMaster4"; GuidPrefix = "F4A"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 12 -or $i.ChecklistNumber -eq 32}; Name = "ActivityMaster5"; GuidPrefix = "F5A"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -in @(5, 31, 32)}; Name = "ActivityMaster6"; GuidPrefix = "F6A"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -in @(7, 31, 32)}; Name = "ActivityMaster7"; GuidPrefix = "F7A"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 8 -or $i.ChecklistNumber -eq 32}; Name = "ActivityMaster8"; GuidPrefix = "F8A"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 16}; Name = "ActivityMaster9"; GuidPrefix = "F9A"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -in @(13, 15, 16, 22)}; Name = "ActivityMaster10"; GuidPrefix = "FAA"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 14 -and ($i.Part -eq 1 -or $i.Part -eq 2)}; Name = "ActivityMaster11"; GuidPrefix = "FBA"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 20}; Name = "ActivityMaster12"; GuidPrefix = "FCA"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 17 -or ($i.ChecklistNumber -eq 18 -and ($i.Part -eq 1 -or $i.Part -eq 2))}; Name = "ActivityMaster13"; GuidPrefix = "FDA"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 6 -and ($i.Part -eq 1 -or $i.Part -eq 3)}; Name = "ActivityMaster14"; GuidPrefix = "FEA"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 11 -or ($i.ChecklistNumber -eq 4 -and ($i.Part -eq 1 -or $i.Part -eq 7))}; Name = "ActivityMaster15"; GuidPrefix = "FFA"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000005"; Filter = {param($i) $i.ChecklistNumber -eq 25}; Name = "ActivityMaster16"; GuidPrefix = "F3B"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000006"; Filter = {param($i) $i.ChecklistNumber -eq 29}; Name = "ActivityMaster17"; GuidPrefix = "F4B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 4}; Name = "ActivityMaster18"; GuidPrefix = "F5B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 3}; Name = "ActivityMaster19"; GuidPrefix = "F6B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -in @(9, 10, 32)}; Name = "ActivityMaster20"; GuidPrefix = "F7B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 27}; Name = "ActivityMaster21"; GuidPrefix = "F8B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000005"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 5}; Name = "ActivityMaster22"; GuidPrefix = "F9B"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000006"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 1}; Name = "ActivityMaster23"; GuidPrefix = "FAB"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000007"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 1}; Name = "ActivityMaster24"; GuidPrefix = "FBB"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000008"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and ($i.Part -eq 2 -or $i.Part -eq 6)}; Name = "ActivityMaster25"; GuidPrefix = "FCB"},
    @{ActivityMasterId = "10000006-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 32 -or $i.ChecklistNumber -eq 33}; Name = "ActivityMaster26"; GuidPrefix = "FDB"}
)

$csCode = @"
using Dubox.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dubox.Infrastructure.Seeding;

/// <summary>
/// Additional seeds for custom ActivityCheckListItem data
/// Generated automatically based on filter criteria
/// </summary>
public static class CustomActivityChecklistItemsSeedData_Part2
{
    private static readonly DateTime SeedDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedAdditionalActivityChecklistItems(ModelBuilder modelBuilder)
    {
"@

foreach ($mapping in $mappings) {
    $filteredItems = $allItems | Where-Object { & $mapping.Filter $_ }
    
    if ($filteredItems.Count -eq 0) {
        Write-Host "WARNING: No items found for $($mapping.Name)"
        continue
    }
    
    Write-Host "Generating code for $($mapping.Name): $($filteredItems.Count) items"
    
    $csCode += @"

        Seed$($mapping.Name)(modelBuilder);
"@
}

$csCode += @"

    }
"@

# Generate individual seed methods
foreach ($mapping in $mappings) {
    $filteredItems = $allItems | Where-Object { & $mapping.Filter $_ }
    
    if ($filteredItems.Count -eq 0) {
        continue
    }
    
    $csCode += @"


    private static void Seed$($mapping.Name)(ModelBuilder modelBuilder)
    {
        var activityMasterId = Guid.Parse("$($mapping.ActivityMasterId)");
        
        var activityCheckListItems = new[]
        {
"@

    $sequence = 1
    foreach ($item in $filteredItems) {
        $guidSuffix = $sequence.ToString("D12")
        $itemGuid = "$($mapping.GuidPrefix)00000-0000-0000-0000-$guidSuffix"
        
        $csCode += @"

            new ActivityCheckListItem
            {
                ActivityCheckListItemId = Guid.Parse("$itemGuid"),
                ActivityMasterId = activityMasterId,
                ActivityTemplateActivityId = null,
                PredefinedChecklistItemId = Guid.Parse("$($item.PredefinedItemId)"),
                Sequence = $sequence,
                IsMandatory = true,
                IsActive = true,
                CreatedDate = SeedDate,
                CreatedBy = "System"
            },
"@
        $sequence++
    }
    
    # Remove last comma
    $csCode = $csCode.TrimEnd(",`r`n") + @"

        };

        modelBuilder.Entity<ActivityCheckListItem>().HasData(activityCheckListItems);
    }
"@
}

$csCode += @"

}
"@

# Write to file
$outputPath = ".\Dubox.Infrastructure\Seeding\CustomActivityChecklistItemsSeedData_Part2.cs"
$csCode | Out-File -FilePath $outputPath -Encoding UTF8
Write-Host "`nC# code generated successfully: $outputPath"
