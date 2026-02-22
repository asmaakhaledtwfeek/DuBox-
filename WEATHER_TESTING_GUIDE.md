# Weather Implementation Testing Guide

## Overview
This guide will help you test the enhanced weather monitoring system with lat/lon coordinates.

## Prerequisites
✅ Database migration applied
✅ OpenWeather API key configured (a9c223a6d364c699179f2e003365a3f3)
✅ Backend and frontend code updated

---

## Testing Steps

### 1. Build and Run Backend API

```bash
cd "c:\Users\asmaa.hassan\source\repos\Digital Engineering"
dotnet build Dubox.Api\Dubox.Api.csproj
dotnet run --project Dubox.Api\Dubox.Api.csproj
```

The API should start at: `https://localhost:7095` (or check console output)

---

### 2. Test Weather API Endpoints Directly

#### Option A: Using Browser/Postman

**Get Weather for a Specific Project:**
```
GET https://localhost:7095/api/projects/weather/{projectId}
```

**Get Weather for All Projects:**
```
GET https://localhost:7095/api/projects/weather/all
```

#### Option B: Using PowerShell

```powershell
# Get weather for a specific project
$projectId = "YOUR_PROJECT_ID_HERE"
Invoke-RestMethod -Uri "https://localhost:7095/api/projects/weather/$projectId" `
  -Method GET `
  -Headers @{"Authorization"="Bearer YOUR_TOKEN"}

# Get all projects weather
Invoke-RestMethod -Uri "https://localhost:7095/api/projects/weather/all" `
  -Method GET `
  -Headers @{"Authorization"="Bearer YOUR_TOKEN"}
```

---

### 3. Trigger Weather Monitoring Manually

You have two options to test the weather monitoring:

#### Option A: Create a Test Controller (Recommended)

Add this temporary test endpoint to test weather fetching:

**File: `Dubox.Api/Controllers/TestWeatherController.cs`**

Check if this file exists and has a method to trigger weather checks manually.

#### Option B: Wait for Scheduled Run

The WeatherMonitoringWorker runs automatically at 7 AM (KSA/UAE time zones).

---

### 4. Create Test Projects with Specific Locations

To test the lat/lon functionality, create projects with these names/codes:

**For Expo 2020 Dubai (25.19°N, 55.27°E):**
- Project Code: "EXPO2020" or anything containing "EXPO"
- Project Name: "Expo 2020 Dubai Project"

**For Rabigh, KSA (22.80°N, 39.04°E):**
- Project Code: "RABIGH01" or anything containing "RABIGH"
- Project Name: "Rabigh Construction Project"

**SQL to Insert Test Projects:**

```sql
-- For Expo 2020 Dubai
INSERT INTO Projects (ProjectId, ProjectCode, ProjectName, Location, Status, IsActive, ActualStartDate, CreatedDate)
VALUES (NEWID(), 'EXPO2020', 'Expo 2020 Dubai Test Project', 1, 1, 1, GETDATE(), GETDATE());

-- For Rabigh KSA
INSERT INTO Projects (ProjectId, ProjectCode, ProjectName, Location, Status, IsActive, ActualStartDate, CreatedDate)
VALUES (NEWID(), 'RABIGH01', 'Rabigh Construction Project', 0, 1, 1, GETDATE(), GETDATE());
```

---

### 5. Run Frontend Application

```bash
cd "c:\Users\asmaa.hassan\source\repos\Digital Engineering\dubox-frontend"
npm start
# or
ng serve
```

The app should start at: `http://localhost:4200`

---

### 6. Test Weather Display in Schedule Dashboard

1. **Navigate to Schedule Dashboard:**
   - Go to: `http://localhost:4200/schedule`

2. **Select a Project:**
   - Choose one of your test projects (EXPO2020 or RABIGH01)
   - Or select any existing active project

3. **Verify Weather Display:**
   
   You should see a Weather Conditions section showing:
   
   - **Temperature Card** (Orange gradient)
     - Current temperature
     - Min/Max range
   
   - **Humidity Card** (Cyan gradient)
     - Humidity percentage
   
   - **Wind Card** (Purple gradient)
     - Wind speed in km/h
     - Wind direction in degrees
   
   - **Pressure Card** (Pink gradient)
     - Atmospheric pressure in hPa
   
   - **Sunrise/Sunset Card** (Yellow gradient)
     - Sunrise time
     - Sunset time
   
   - **Location Card** (Green gradient)
     - GPS coordinates (degrees, minutes, seconds)
     - Elevation in meters
   
   - **Description Card** (Blue gradient, spans 2 columns)
     - Weather description
     - Alert if unfavorable for construction

---

### 7. Verify Data in Database

Check if weather data is being saved:

```sql
-- View all weather reports
SELECT TOP 10 
    p.ProjectCode,
    p.ProjectName,
    w.ReportDate,
    w.CurrentTemperature,
    w.Humidity,
    w.WindSpeed,
    w.Pressure,
    w.Latitude,
    w.Longitude,
    w.Description,
    w.IsFavorable
FROM ProjectWeatherReports w
INNER JOIN Projects p ON w.ProjectId = p.ProjectId
ORDER BY w.ReportDate DESC;

-- Check specific coordinates for Expo/Rabigh projects
SELECT 
    p.ProjectCode,
    w.Latitude,
    w.Longitude,
    w.CurrentTemperature,
    w.Sunrise,
    w.Sunset
FROM ProjectWeatherReports w
INNER JOIN Projects p ON w.ProjectId = p.ProjectId
WHERE p.ProjectCode LIKE '%EXPO%' OR p.ProjectCode LIKE '%RABIGH%';
```

---

### 8. Test Weather Service Directly (Optional)

You can also test the weather service directly using a test endpoint.

**Create a temporary test endpoint:**

```csharp
[HttpGet("test-coordinates")]
[AllowAnonymous]
public async Task<IActionResult> TestWeatherByCoordinates(
    [FromQuery] decimal lat = 25.19m, 
    [FromQuery] decimal lon = 55.27m)
{
    var weatherService = HttpContext.RequestServices
        .GetRequiredService<IWeatherService>();
    
    var weather = await weatherService
        .GetWeatherForecastByCoordinatesAsync(lat, lon);
    
    return Ok(weather);
}
```

**Test it:**
```
GET https://localhost:7095/api/projects/weather/test-coordinates?lat=25.19&lon=55.27
```

---

## Expected Results

### ✅ Success Indicators:

1. **API Response** should include:
   - `currentTemperature`
   - `humidity`
   - `windSpeed`, `windDirection`
   - `pressure`
   - `sunrise`, `sunset`
   - `latitude`, `longitude`
   - All values populated (not zeros)

2. **Frontend Display** should show:
   - 8 colorful gradient cards
   - Real weather data (not placeholder values)
   - Properly formatted times and coordinates
   - Loading spinner while fetching data

3. **Database** should contain:
   - New rows in `ProjectWeatherReports` table
   - All new columns populated with data
   - Correct lat/lon for Expo (25.19, 55.27) and Rabigh (22.80, 39.04)

### ❌ Common Issues:

1. **"No weather report available"**
   - Solution: Run the WeatherMonitoringWorker or wait until 7 AM
   - Or: Manually call the weather API endpoint

2. **All weather values are zero**
   - Issue: OpenWeather API call failed
   - Check: API key is valid
   - Check: Internet connection
   - Check: API response in logs

3. **Weather section not showing**
   - Check: Project is selected
   - Check: Browser console for errors
   - Check: API is running and accessible

4. **Wrong coordinates**
   - Check: Project name/code contains "EXPO" or "RABIGH"
   - Verify: Database has correct lat/lon values

---

## Quick Test Script

Here's a complete PowerShell script to test everything:

```powershell
# 1. Build backend
Write-Host "Building backend..." -ForegroundColor Cyan
dotnet build "Dubox.Api\Dubox.Api.csproj"

# 2. Check if migration is applied
Write-Host "`nChecking database migration..." -ForegroundColor Cyan
dotnet ef database update --startup-project "Dubox.Api\Dubox.Api.csproj" --project "Dubox.Infrastructure\Dubox.Infrastructure.csproj"

# 3. Start backend in background
Write-Host "`nStarting backend API..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'c:\Users\asmaa.hassan\source\repos\Digital Engineering'; dotnet run --project Dubox.Api\Dubox.Api.csproj"

# 4. Wait for API to start
Write-Host "Waiting for API to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# 5. Start frontend
Write-Host "`nStarting frontend..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'c:\Users\asmaa.hassan\source\repos\Digital Engineering\dubox-frontend'; npm start"

Write-Host "`n✅ Both servers are starting!" -ForegroundColor Green
Write-Host "Backend: https://localhost:7095" -ForegroundColor White
Write-Host "Frontend: http://localhost:4200" -ForegroundColor White
Write-Host "`nNavigate to: http://localhost:4200/schedule and select a project" -ForegroundColor Yellow
```

---

## Troubleshooting

### Backend Issues:

```bash
# Check if API is running
curl https://localhost:7095/api/projects/weather/all

# View logs
# Check console output for any errors
```

### Frontend Issues:

```bash
# Check browser console (F12)
# Look for network errors or JavaScript errors

# Restart frontend with verbose logging
ng serve --verbose
```

### Database Issues:

```sql
-- Check if new columns exist
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ProjectWeatherReports'
ORDER BY ORDINAL_POSITION;

-- Should include: CurrentTemperature, Humidity, WindSpeed, WindDirection, 
-- Pressure, Sunrise, Sunset, Latitude, Longitude, Elevation
```

---

## Next Steps After Testing

Once testing is complete and successful:

1. ✅ Remove test endpoints (if created)
2. ✅ Add proper error handling
3. ✅ Configure production API keys
4. ✅ Set up proper logging
5. ✅ Document API endpoints in Swagger

---

## Contact & Support

If you encounter issues during testing:
- Check the console logs for detailed error messages
- Verify all services are running
- Ensure database connection is working
- Validate OpenWeather API key is active

Good luck with testing! 🌤️
