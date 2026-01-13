# Apply responsive design to all component SCSS files
# Run from: dubox-frontend directory

Write-Host "Starting responsive design application..." -ForegroundColor Green

$responsiveBlock = Get-Content -Path "responsive-template.txt" -Raw

$scssFiles = Get-ChildItem -Path "src\app\features" -Filter "*.component.scss" -Recurse

Write-Host "Found $($scssFiles.Count) SCSS files" -ForegroundColor Cyan

$updated = 0
$skipped = 0

foreach ($file in $scssFiles) {
    try {
        $content = Get-Content $file.FullName -Raw
        
        if ($content -notmatch "RESPONSIVE DESIGN - AUTO-GENERATED") {
            Write-Host "Updating: $($file.Name)" -ForegroundColor Yellow
            
            $newContent = $content.TrimEnd() + "`n`n" + $responsiveBlock
            Set-Content -Path $file.FullName -Value $newContent -NoNewline -Encoding UTF8
            
            $updated++
            Write-Host "  ✓ Updated successfully" -ForegroundColor Green
        } else {
            $skipped++
            Write-Host "  ⊘ Already responsive: $($file.Name)" -ForegroundColor DarkGray
        }
    } catch {
        Write-Host "  ✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`nCompleted!" -ForegroundColor Green
Write-Host "Updated: $updated files" -ForegroundColor Cyan
Write-Host "Skipped: $skipped files" -ForegroundColor Yellow

