# Script to update Checklist 32 with new data from new 32 checklist.json

$mainJsonPath = ".\Dubox.Infrastructure\Seeding\checklist_hierarchy_with_guids.json"
$newChecklist32Path = ".\Documentation\new 32 checklist.json"

# Read both JSON files
$mainJson = Get-Content $mainJsonPath -Raw | ConvertFrom-Json
$newChecklist32Data = Get-Content $newChecklist32Path -Raw | ConvertFrom-Json

Write-Host "Reading existing checklist data..."
Write-Host "Total checklists in main file: $($mainJson.checklists.Count)"

# Find checklist 32 in the main JSON (by code CHK-PC-032)
$checklist32Index = -1
for ($i = 0; $i -lt $mainJson.checklists.Count; $i++) {
    if ($mainJson.checklists[$i].code -like "CHK-PC-032*") {
        $checklist32Index = $i
        Write-Host "Found existing Checklist 32 at index $i with code: $($mainJson.checklists[$i].code)"
        break
    }
}

if ($checklist32Index -eq -1) {
    Write-Host "ERROR: Checklist 32 (CHK-PC-032) not found in main JSON!"
    exit 1
}

# Read the existing checklist 32 to preserve GUIDs
$existingChecklist32 = $mainJson.checklists[$checklist32Index]
$existingChecklistId = $existingChecklist32.checklistId

Write-Host "`nExisting Checklist 32 details:"
Write-Host "  ChecklistId: $existingChecklistId"
Write-Host "  Name: $($existingChecklist32.name)"
Write-Host "  Sections: $($existingChecklist32.sections.Count)"

# Build a map of existing item GUIDs by description or sequence
$existingItemsMap = @{}
foreach ($section in $existingChecklist32.sections) {
    foreach ($item in $section.items) {
        $key = "$($item.description.Trim().ToLower())"
        if (-not $existingItemsMap.ContainsKey($key)) {
            $existingItemsMap[$key] = $item.predefinedItemId
        }
    }
}

Write-Host "Mapped $($existingItemsMap.Count) existing items by description"

# Process the new checklist 32 data
$updatedSections = @()
$sectionOrder = 1
$overallSequence = 1

foreach ($newChecklistData in $newChecklist32Data) {
    Write-Host "`n Processing checklist: $($newChecklistData.name)"
    
    foreach ($newSection in $newChecklistData.sections) {
        $sectionId = [Guid]::NewGuid().ToString()
        
        $updatedItems = @()
        foreach ($newItem in $newSection.items) {
            # Try to find existing GUID for this item
            $itemKey = $newItem.description.Trim().ToLower()
            $itemGuid = $null
            
            if ($existingItemsMap.ContainsKey($itemKey)) {
                $itemGuid = $existingItemsMap[$itemKey]
                Write-Host "  Reusing GUID for: $($newItem.description.Substring(0, [Math]::Min(50, $newItem.description.Length)))..."
            } else {
                $itemGuid = [Guid]::NewGuid().ToString()
                Write-Host "  New GUID for: $($newItem.description.Substring(0, [Math]::Min(50, $newItem.description.Length)))..."
            }
            
            $updatedItems += [PSCustomObject]@{
                predefinedItemId = $itemGuid
                description = $newItem.description
                sequence = $overallSequence
                isActive = $true
                createdDate = "2025-02-10T00:00:00"
                checklistSectionId = $sectionId
                reference = "REF-$(($overallSequence).ToString('D4'))"
                checklistNumber = 32
                part = $newItem.part
            }
            $overallSequence++
        }
        
        $updatedSections += [PSCustomObject]@{
            checklistSectionId = $sectionId
            title = $newSection.title
            order = $sectionOrder
            checklistId = $existingChecklistId
            isActive = $true
            createdDate = "2025-02-10T00:00:00"
            items = $updatedItems
        }
        $sectionOrder++
    }
}

# Update the checklist 32 in main JSON
$mainJson.checklists[$checklist32Index].name = "Check List for Pre-Loading of Completed Precast Modular (MEP & Electrical)"
$mainJson.checklists[$checklist32Index].sections = $updatedSections
$mainJson.checklists[$checklist32Index].pageNumber = 32

Write-Host "`nUpdated Checklist 32:"
Write-Host "  New sections: $($updatedSections.Count)"
Write-Host "  Total items: $(($updatedSections | ForEach-Object { $_.items.Count } | Measure-Object -Sum).Sum)"

# Save the updated JSON
$outputJson = $mainJson | ConvertTo-Json -Depth 20
$outputJson | Out-File $mainJsonPath -Encoding UTF8

Write-Host "`n✅ Successfully updated $mainJsonPath"
Write-Host "`nNext steps:"
Write-Host "1. Review the updated JSON file"
Write-Host "2. Create a new migration to update the database"
Write-Host "3. Apply the migration"
