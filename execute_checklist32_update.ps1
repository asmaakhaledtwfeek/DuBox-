# Script to update Checklist 32 in the database

Write-Host "=== Checklist 32 Database Update ===" -ForegroundColor Cyan
Write-Host ""

# Get connection string from appsettings
$appsettingsPath = ".\Dubox.Api\appsettings.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
    $connectionString = $appsettings.ConnectionStrings.DefaultConnection
    
    if ($connectionString) {
        Write-Host "Found connection string in appsettings.json" -ForegroundColor Green
        
        # Execute the SQL script
        $sqlScript = Get-Content ".\update_checklist_32_database.sql" -Raw
        
        try {
            # Use SqlClient to execute
            Add-Type -AssemblyName "System.Data"
            $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
            $connection.Open()
            
            $command = $connection.CreateCommand()
            $command.CommandText = $sqlScript
            $command.CommandTimeout = 300 # 5 minutes
            
            Write-Host "Executing SQL script..." -ForegroundColor Yellow
            $rowsAffected = $command.ExecuteNonQuery()
            
            $connection.Close()
            
            Write-Host ""
            Write-Host "✅ SQL script executed successfully!" -ForegroundColor Green
            Write-Host ""
            Write-Host "Next steps:" -ForegroundColor Cyan
            Write-Host "1. Create migration: dotnet ef migrations add InsertNewChecklist32Data --project Dubox.Infrastructure --startup-project Dubox.Api"
            Write-Host "2. Apply migration: dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api"
        }
        catch {
            Write-Host "❌ Error executing SQL script: $_" -ForegroundColor Red
            Write-Host ""
            Write-Host "Alternative: Run the SQL script manually in SQL Server Management Studio or Azure Data Studio"
            Write-Host "Script location: .\update_checklist_32_database.sql"
        }
    }
    else {
        Write-Host "❌ Connection string not found in appsettings.json" -ForegroundColor Red
    }
}
else {
    Write-Host "❌ appsettings.json not found" -ForegroundColor Red
}
