# PowerShell script to convert integer IDs to GUIDs in JSON file

$inputFile = "Documentation\checklist_hierarchy_fixed.json"
$outputFile = "Documentation\checklist_hierarchy_with_guids.json"

Write-Host "Reading $inputFile..." -ForegroundColor Cyan

# Read the JSON file
$jsonContent = Get-Content -Path $inputFile -Raw -Encoding UTF8
$data = $jsonContent | ConvertFrom-Json

Write-Host "Converting IDs to GUIDs..." -ForegroundColor Cyan

# Counters
$checklistCount = 0
$sectionCount = 0
$itemCount = 0

# Process each checklist
foreach ($checklist in $data.checklists) {
    # Generate new GUID for checklist
    $oldChecklistId = $checklist.checklistId
    $newChecklistId = [guid]::NewGuid().ToString()
    $checklist.checklistId = $newChecklistId
    $checklistCount++
    
    # Process sections
    if ($checklist.sections) {
        foreach ($section in $checklist.sections) {
            # Generate new GUID for section
            $oldSectionId = $section.checklistSectionId
            $newSectionId = [guid]::NewGuid().ToString()
            $section.checklistSectionId = $newSectionId
            $section.checklistId = $newChecklistId  # Update foreign key
            $sectionCount++
            
            # Process items
            if ($section.items) {
                foreach ($item in $section.items) {
                    # Generate new GUID for item
                    $newItemId = [guid]::NewGuid().ToString()
                    $item.predefinedItemId = $newItemId
                    $item.checklistSectionId = $newSectionId  # Update foreign key
                    $itemCount++
                }
            }
        }
    }
}

Write-Host "Writing converted data to $outputFile..." -ForegroundColor Cyan

# Convert back to JSON with proper formatting
$outputJson = $data | ConvertTo-Json -Depth 100 -Compress:$false

# Write to output file
$outputJson | Out-File -FilePath $outputFile -Encoding UTF8

Write-Host "`nConversion completed successfully!" -ForegroundColor Green
Write-Host "Total checklists converted: $checklistCount" -ForegroundColor Yellow
Write-Host "Total sections converted: $sectionCount" -ForegroundColor Yellow
Write-Host "Total items converted: $itemCount" -ForegroundColor Yellow
Write-Host "`nSuccessfully created $outputFile" -ForegroundColor Green
