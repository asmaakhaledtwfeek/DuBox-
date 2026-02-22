# Script to extract predefined checklist items based on filter criteria
# This will help generate seeding data for ActivityCheckListItems

$jsonPath = ".\Dubox.Infrastructure\Seeding\checklist_hierarchy_with_guids.json"
$json = Get-Content $jsonPath -Raw | ConvertFrom-Json

# Extract all predefined items with their ChecklistNumber and Part
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

# Define the filter criteria for each Activity Master
$mappings = @(
    @{ActivityMasterId = "10000001-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 2}; Name = "ActivityMaster3"},
    @{ActivityMasterId = "10000001-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 1 -or $i.ChecklistNumber -eq 3}; Name = "ActivityMaster4"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 12 -or $i.ChecklistNumber -eq 32}; Name = "ActivityMaster5"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -in @(5, 31, 32)}; Name = "ActivityMaster6"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -in @(7, 31, 32)}; Name = "ActivityMaster7"},
    @{ActivityMasterId = "10000002-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 8 -or $i.ChecklistNumber -eq 32}; Name = "ActivityMaster8"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 16}; Name = "ActivityMaster9"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -in @(13, 15, 16, 22)}; Name = "ActivityMaster10"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 14 -and ($i.Part -eq 1 -or $i.Part -eq 2)}; Name = "ActivityMaster11"},
    @{ActivityMasterId = "10000003-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 20}; Name = "ActivityMaster12"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 17 -or ($i.ChecklistNumber -eq 18 -and ($i.Part -eq 1 -or $i.Part -eq 2))}; Name = "ActivityMaster13"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 6 -and ($i.Part -eq 1 -or $i.Part -eq 3)}; Name = "ActivityMaster14"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -eq 11 -or ($i.ChecklistNumber -eq 4 -and ($i.Part -eq 1 -or $i.Part -eq 7))}; Name = "ActivityMaster15"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000005"; Filter = {param($i) $i.ChecklistNumber -eq 25}; Name = "ActivityMaster16"},
    @{ActivityMasterId = "10000004-0000-0000-0000-000000000006"; Filter = {param($i) $i.ChecklistNumber -eq 29}; Name = "ActivityMaster17"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000001"; Filter = {param($i) $i.ChecklistNumber -eq 29 -and $i.Part -eq 4}; Name = "ActivityMaster18"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 3}; Name = "ActivityMaster19"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000003"; Filter = {param($i) $i.ChecklistNumber -in @(9, 10, 32)}; Name = "ActivityMaster20"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000004"; Filter = {param($i) $i.ChecklistNumber -eq 27}; Name = "ActivityMaster21"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000005"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 5}; Name = "ActivityMaster22"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000006"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 1}; Name = "ActivityMaster23"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000007"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and $i.Part -eq 1}; Name = "ActivityMaster24"},
    @{ActivityMasterId = "10000005-0000-0000-0000-000000000008"; Filter = {param($i) $i.ChecklistNumber -eq 32 -and ($i.Part -eq 2 -or $i.Part -eq 6)}; Name = "ActivityMaster25"},
    @{ActivityMasterId = "10000006-0000-0000-0000-000000000002"; Filter = {param($i) $i.ChecklistNumber -eq 32 -or $i.ChecklistNumber -eq 33}; Name = "ActivityMaster26"}
)

# Generate output for each mapping
$output = @()
foreach ($mapping in $mappings) {
    $filteredItems = $allItems | Where-Object { & $mapping.Filter $_ }
    
    $output += [PSCustomObject]@{
        ActivityMasterId = $mapping.ActivityMasterId
        Name = $mapping.Name
        Count = $filteredItems.Count
        Items = $filteredItems
    }
    
    Write-Host "`n=== $($mapping.Name) ($($mapping.ActivityMasterId)) ==="
    Write-Host "Found $($filteredItems.Count) matching items"
    foreach ($item in $filteredItems) {
        Write-Host "  - $($item.PredefinedItemId) (ChecklistNumber=$($item.ChecklistNumber), Part=$($item.Part))"
    }
}

# Export to JSON for code generation
$output | ConvertTo-Json -Depth 10 | Out-File "checklist_mappings_output.json"
Write-Host "`n`nResults exported to checklist_mappings_output.json"
