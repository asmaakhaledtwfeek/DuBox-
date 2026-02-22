# Checklist JSON Verification Script
param()

$ErrorActionPreference = "Stop"
$jsonFile = "Dubox.Infrastructure\Seeding\checklist_hierarchy_with_guids.json"

Write-Host ""
Write-Host "=== Checklist JSON Verification ===" -ForegroundColor Cyan
Write-Host ""

# Check file exists
if (-not (Test-Path $jsonFile)) {
    Write-Host "ERROR: JSON file not found at: $jsonFile" -ForegroundColor Red
    exit 1
}

Write-Host "[OK] JSON file exists" -ForegroundColor Green
$fileInfo = Get-Item $jsonFile
Write-Host "     Location: $jsonFile" -ForegroundColor Gray
Write-Host "     Size: $($fileInfo.Length) bytes" -ForegroundColor Gray
Write-Host ""

# Parse JSON
Write-Host "Parsing JSON..." -ForegroundColor Cyan
try {
    $jsonContent = Get-Content -Path $jsonFile -Raw -Encoding UTF8
    $data = $jsonContent | ConvertFrom-Json
    Write-Host "[OK] JSON is valid" -ForegroundColor Green
}
catch {
    Write-Host "ERROR: Failed to parse JSON" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Validating structure..." -ForegroundColor Cyan

if (-not $data.checklists) {
    Write-Host "ERROR: Missing checklists property" -ForegroundColor Red
    exit 1
}

$checklistCount = 0
$sectionCount = 0
$itemCount = 0

foreach ($checklist in $data.checklists) {
    $checklistCount++
    
    if ($checklist.sections) {
        foreach ($section in $checklist.sections) {
            $sectionCount++
            
            if ($section.items) {
                $itemCount += $section.items.Count
            }
        }
    }
}

Write-Host "[OK] Structure is valid" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Checklists: $checklistCount" -ForegroundColor Yellow
Write-Host "Sections: $sectionCount" -ForegroundColor Yellow
Write-Host "Items: $itemCount" -ForegroundColor Yellow
Write-Host ""
Write-Host "[OK] All validations passed!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Create migration:" -ForegroundColor Gray
Write-Host "   dotnet ef migrations add SeedChecklistsFromJson --project Dubox.Infrastructure --startup-project Dubox.Api" -ForegroundColor White
Write-Host ""
Write-Host "2. Apply migration:" -ForegroundColor Gray
Write-Host "   dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api" -ForegroundColor White
Write-Host ""
