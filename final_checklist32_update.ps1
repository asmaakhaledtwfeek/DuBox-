# Final script to update Checklist 32 - executes SQL directly using SqlCmd or provides manual instructions

Write-Host "=== Checklist 32 Database Update ===" -ForegroundColor Cyan
Write-Host ""

# Try to execute using sqlcmd if available
$sqlScriptPath = ".\update_checklist_32_database.sql"

try {
    # Check if sqlcmd is available
    $sqlcmdPath = (Get-Command sqlcmd -ErrorAction SilentlyContinue).Source
    
    if ($sqlcmdPath) {
        Write-Host "Found sqlcmd at: $sqlcmdPath" -ForegroundColor Green
        Write-Host "Attempting to execute SQL script using LocalDB..." -ForegroundColor Yellow
        Write-Host ""
        
        # Execute against LocalDB
        sqlcmd -S "(localdb)\MSSQLLocalDB" -d Dubox -i $sqlScriptPath -E
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host ""
            Write-Host "✅ SQL script executed successfully!" -ForegroundColor Green
        } else {
            Write-Host ""
            Write-Host "⚠️ sqlcmd returned exit code: $LASTEXITCODE" -ForegroundColor Yellow
            Write-Host "The script may have executed with some warnings." -ForegroundColor Yellow
        }
    } else {
        throw "sqlcmd not found"
    }
}
catch {
    Write-Host "⚠️ Could not execute SQL automatically: $_" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "📋 MANUAL STEPS REQUIRED:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. Open SQL Server Management Studio or Azure Data Studio"
    Write-Host "2. Connect to: (localdb)\MSSQLLocalDB"
    Write-Host "3. Open the file: update_checklist_32_database.sql"
    Write-Host "4. Execute the script against the 'Dubox' database"
    Write-Host ""
    Write-Host "OR execute this command in PowerShell with appropriate permissions:"
    Write-Host "sqlcmd -S `"(localdb)\MSSQLLocalDB`" -d Dubox -i `"$sqlScriptPath`" -E" -ForegroundColor White
    exit 1
}

Write-Host ""
Write-Host "=== Next Steps ===" -ForegroundColor Cyan
Write-Host "1. Create migration to capture new data:" -ForegroundColor White
Write-Host "   dotnet ef migrations add InsertNewChecklist32Data --project Dubox.Infrastructure --startup-project Dubox.Api"
Write-Host ""
Write-Host "2. Apply the migration:" -ForegroundColor White
Write-Host "   dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api"
Write-Host ""
