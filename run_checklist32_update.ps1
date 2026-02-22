# Simple script to execute SQL update for Checklist 32

Write-Host "=== Checklist 32 Database Update ===" -ForegroundColor Cyan
Write-Host ""

$sqlScript = ".\update_checklist_32_database.sql"

# Try using sqlcmd
try {
    Write-Host "Executing SQL script using sqlcmd..." -ForegroundColor Yellow
    $output = sqlcmd -S "(localdb)\MSSQLLocalDB" -d Dubox -i $sqlScript -E 2>&1
    
    Write-Host $output
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "SUCCESS! SQL script executed." -ForegroundColor Green
        Write-Host ""
        Write-Host "Next: Run these commands:" -ForegroundColor Cyan
        Write-Host "  dotnet ef migrations add InsertNewChecklist32Data --project Dubox.Infrastructure --startup-project Dubox.Api"
        Write-Host "  dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api"
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please run the SQL script manually:" -ForegroundColor Yellow
    Write-Host "File: update_checklist_32_database.sql"
}
