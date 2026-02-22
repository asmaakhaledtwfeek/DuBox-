# Weather Implementation Test Script
# This script helps you quickly test the weather implementation

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Weather Implementation Test Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Apply Migration
Write-Host "Step 1: Applying database migration..." -ForegroundColor Yellow
try {
    dotnet ef database update --startup-project "Dubox.Api\Dubox.Api.csproj" --project "Dubox.Infrastructure\Dubox.Infrastructure.csproj" 2>&1 | Out-Null
    Write-Host "✅ Migration applied successfully" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Migration may have already been applied or there was an error" -ForegroundColor Yellow
}
Write-Host ""

# Step 2: Build Backend
Write-Host "Step 2: Building backend..." -ForegroundColor Yellow
$buildResult = dotnet build "Dubox.Api\Dubox.Api.csproj" --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Backend built successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Backend build failed" -ForegroundColor Red
    exit
}
Write-Host ""

# Step 3: Start Backend
Write-Host "Step 3: Starting backend API..." -ForegroundColor Yellow
Write-Host "   Opening new terminal window for backend..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", @"
    Write-Host 'Starting Dubox API...' -ForegroundColor Cyan
    cd 'c:\Users\asmaa.hassan\source\repos\Digital Engineering'
    dotnet run --project Dubox.Api\Dubox.Api.csproj
"@
Write-Host "✅ Backend starting in new window" -ForegroundColor Green
Write-Host ""

# Step 4: Wait for API
Write-Host "Step 4: Waiting for API to initialize (15 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 15
Write-Host "✅ API should be ready now" -ForegroundColor Green
Write-Host ""

# Step 5: Start Frontend
Write-Host "Step 5: Starting frontend..." -ForegroundColor Yellow
Write-Host "   Opening new terminal window for frontend..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", @"
    Write-Host 'Starting Angular Frontend...' -ForegroundColor Cyan
    cd 'c:\Users\asmaa.hassan\source\repos\Digital Engineering\dubox-frontend'
    npm start
"@
Write-Host "✅ Frontend starting in new window" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Test Environment Ready!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "🌐 Backend API:  " -NoNewline -ForegroundColor White
Write-Host "https://localhost:7095" -ForegroundColor Yellow
Write-Host "🌐 Frontend App: " -NoNewline -ForegroundColor White
Write-Host "http://localhost:4200" -ForegroundColor Yellow
Write-Host ""
Write-Host "📋 Testing Steps:" -ForegroundColor Cyan
Write-Host "   1. Wait for frontend to compile (1-2 minutes)" -ForegroundColor White
Write-Host "   2. Open browser: http://localhost:4200" -ForegroundColor White
Write-Host "   3. Login to the application" -ForegroundColor White
Write-Host "   4. Navigate to: Schedule Dashboard" -ForegroundColor White
Write-Host "   5. Select a project from the dropdown" -ForegroundColor White
Write-Host "   6. View the Weather section below project details" -ForegroundColor White
Write-Host ""
Write-Host "🔍 What to Look For:" -ForegroundColor Cyan
Write-Host "   ✓ 8 colorful weather cards" -ForegroundColor White
Write-Host "   ✓ Temperature, Humidity, Wind, Pressure" -ForegroundColor White
Write-Host "   ✓ Sunrise/Sunset times" -ForegroundColor White
Write-Host "   ✓ GPS Coordinates" -ForegroundColor White
Write-Host "   ✓ Weather description" -ForegroundColor White
Write-Host ""
Write-Host "📊 Test Projects:" -ForegroundColor Cyan
Write-Host "   • Projects with 'EXPO' in name → Dubai coords (25.19°N, 55.27°E)" -ForegroundColor White
Write-Host "   • Projects with 'RABIGH' in name → Rabigh coords (22.80°N, 39.04°E)" -ForegroundColor White
Write-Host ""
Write-Host "📖 For detailed testing guide, see:" -ForegroundColor Cyan
Write-Host "   WEATHER_TESTING_GUIDE.md" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press any key to open the testing guide..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
Start-Process "WEATHER_TESTING_GUIDE.md"
