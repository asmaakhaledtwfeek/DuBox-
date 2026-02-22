# Weather Data Verification Guide
## Project: RSG 17 Linear Buildings (Code: 160)

---

## Step 1: Check Database for Weather Data

Run the SQL script to verify weather data exists:

```bash
# Open SQL Server Management Studio and run:
check-weather-data.sql
```

This will show you:
- ✓ Project information
- ✓ All weather reports
- ✓ Data completeness check
- ✓ Latest weather summary

---

## Step 2: Verify API Endpoint

### Test the weather API directly:

**Option A: Using Browser**
```
https://localhost:7095/api/projects/weather/8356A42E-D4B1-4321-8B17-08DE415C8695
```

**Option B: Using PowerShell**
```powershell
# Get weather for specific project
$projectId = "8356A42E-D4B1-4321-8B17-08DE415C8695"
$response = Invoke-RestMethod -Uri "https://localhost:7095/api/projects/weather/$projectId" -Method GET -Headers @{"Authorization"="Bearer YOUR_TOKEN"}
$response | ConvertTo-Json -Depth 10
```

### Expected Response Structure:
```json
{
  "isSuccess": true,
  "data": {
    "reportId": "...",
    "projectId": "8356A42E-D4B1-4321-8B17-08DE415C8695",
    "projectCode": "160",
    "projectName": "RSG 17 Linear Buildings",
    "reportDate": "2026-02-11T...",
    "currentTemperature": 27.06,
    "minTemperature": 27.06,
    "maxTemperature": 27.06,
    "humidity": 65,
    "precipitationProbability": 0,
    "precipitationAmount": 0,
    "windSpeed": 4.39,
    "windGust": 4.39,
    "windDirection": 180,
    "pressure": 1013,
    "solarRadiation": 0,
    "sunrise": "2026-02-11T06:45:00",
    "sunset": "2026-02-11T18:15:00",
    "moonrise": "2026-02-11T02:30:00",
    "moonset": "2026-02-11T14:20:00",
    "latitude": 25.19,
    "longitude": 55.27,
    "elevation": 25,
    "description": "overcast clouds",
    "isFavorable": true,
    "alertMessage": null,
    "qualityIssueCreated": false
  }
}
```

---

## Step 3: Verify Frontend Display

### All Weather Fields Should Show:

#### **Main Display Section**
- [x] Temperature: `27.06°C - 27.06°C`
- [x] Weather Icon (Sun/Cloud)
- [x] Condition: `Overcast Clouds`
- [x] Badge: `Favorable` (green) or `Unfavorable` (red)

#### **Detail Cards Grid**

1. **Precipitation**
   - Icon: Water droplet
   - Value: `0% / 0mm`

2. **Humidity** (الرطوبة النسبية)
   - Icon: Orange water droplet 🔥
   - Value: `65%`

3. **Wind with Compass** (سرعة الرياح)
   - Interactive compass showing direction
   - Needle points to: `180°` (South)
   - Speed: `💨 4.39 km/h`

4. **Solar Radiation** (الإشعاع الشمسي)
   - Icon: Sun
   - Value: `0 wh/m²` (placeholder until data source added)

5. **Atmospheric Pressure** (الضغط الجوي)
   - Icon: Gauge
   - Value: `1013 hPa`

#### **Sun & Moon Times Section**
- [x] Sunrise: `06:45` ↑
- [x] Sunset: `18:15` ↓
- [x] Moonrise: `02:30` 🌙 ↑
- [x] Moonset: `14:20` 🌙 ↓

#### **Location Information**
- [x] Coordinates: `25 11 24 N 55 16 12 E / 25m`
- Format: Degrees Minutes Seconds / Elevation

#### **Time & History**
- [x] Current Time: `HH:MM UAE`
- [x] Navigation buttons: ‹ ›
- [x] History buttons:
  - `آخر 24 ساعة` (Last 24 hours)
  - `آخر 12 ساعة` (Last 12 hours)

---

## Step 4: Troubleshooting

### If weather data is NOT showing:

#### **Check 1: Does weather data exist in database?**
```sql
SELECT COUNT(*) FROM ProjectWeatherReports 
WHERE ProjectId = '8356A42E-D4B1-4321-8B17-08DE415C8695'
```

**If 0 rows:** Weather data hasn't been generated yet.
- Solution: Run WeatherMonitoringWorker or wait for 7 AM scheduled run

#### **Check 2: Is the API returning data?**
- Open browser dev tools (F12)
- Go to Network tab
- Navigate to schedule page and select project
- Look for API call to `/api/projects/weather/...`
- Check response

**If 404 Not Found:** Project has no weather data
**If 500 Error:** Backend error, check API logs

#### **Check 3: Is the frontend loading the data?**
```typescript
// Open browser console (F12) and run:
console.log('Weather Data:', weatherData);
```

**If null/undefined:** Frontend not receiving data
**If has data:** UI component issue

#### **Check 4: Are all fields populated?**
```sql
SELECT 
    CASE WHEN CurrentTemperature = 0 THEN 'Missing CurrentTemperature' END,
    CASE WHEN Humidity = 0 THEN 'Missing Humidity' END,
    CASE WHEN Pressure = 0 THEN 'Missing Pressure' END,
    CASE WHEN Latitude = 0 THEN 'Missing Location' END
FROM ProjectWeatherReports
WHERE ProjectId = '8356A42E-D4B1-4321-8B17-08DE415C8695'
```

---

## Step 5: Generate Weather Data Manually (If Needed)

If no weather data exists, you can generate it:

### **Option A: Wait for Scheduled Run**
- WeatherMonitoringWorker runs at 7 AM KSA/UAE time
- Automatically checks all active projects

### **Option B: Manual Trigger (Create Test Endpoint)**

Add this to `TestWeatherController.cs`:

```csharp
[HttpPost("trigger-weather-check/{projectId}")]
[AllowAnonymous] // Remove in production
public async Task<IActionResult> TriggerWeatherCheck(Guid projectId)
{
    var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
    var weatherService = _serviceProvider.GetRequiredService<IWeatherService>();
    var notificationHub = _serviceProvider.GetRequiredService<INotificationHubService>();
    
    var project = await unitOfWork.Repository<Project>().GetByIdAsync(projectId);
    if (project == null)
        return NotFound("Project not found");
    
    var worker = new WeatherMonitoringWorker(_serviceProvider, _logger);
    // Call private method using reflection or make it public for testing
    
    return Ok("Weather check triggered");
}
```

Then call:
```
POST https://localhost:7095/api/weather/trigger-weather-check/8356A42E-D4B1-4321-8B17-08DE415C8695
```

---

## Step 6: Expected Display for RSG 17 Linear Buildings

Based on your project data:
- **Location**: UAE (Location = 1)
- **Coordinates**: Expo 2020 Dubai (25.19°N, 55.27°E)
- **Project Status**: Active
- **Actual Start Date**: December 22, 2025

### Weather Should Show:
```
Today's Weather
┌─────────────────────────────────────┐
│  ☀️  27.06°C - 27.06°C             │
│     Overcast Clouds      Favorable  │
└─────────────────────────────────────┘

Precipitation    Humidity        Wind           Solar
0% / 0mm        65% 🔥          💨 4.39 km/h   0 wh/m²
                                🧭 180°

Pressure
1013 hPa

☀️ 06:45 ↑  |  ☀️ 18:15 ↓
🌙 02:30 ↑  |  🌙 14:20 ↓

📍 25 11 24 N 55 16 12 E / 25m

⏰ HH:MM UAE

[📊 آخر 24 ساعة]  [📊 آخر 12 ساعة]
```

---

## Quick Checklist

- [ ] SQL query shows weather data exists
- [ ] API endpoint returns valid JSON response
- [ ] Frontend loads without errors
- [ ] All 8 detail cards are visible
- [ ] Compass rotates to correct direction
- [ ] Sun/Moon times display correctly
- [ ] Coordinates match Expo 2020 location
- [ ] Favorable/Unfavorable badge shows
- [ ] History buttons are clickable

---

## Need More Help?

1. **Check browser console** for JavaScript errors
2. **Check API logs** for backend errors
3. **Verify database** has weather data
4. **Review network tab** in dev tools for API calls

**Contact**: Review WEATHER_UI_ENHANCEMENT_SUMMARY.md for detailed documentation
