# Quick Test Guide - Live Weather Integration

## What Changed

✅ **Single Project Weather Endpoint** now fetches **LIVE** weather data from OpenWeather API  
✅ **Background Job** continues to save weather data for notifications and quality issues  
✅ **All Projects Endpoint** still uses cached database data (for performance)

## Files Modified

1. **GetProjectWeatherReportQueryHandler.cs** - Updated to fetch live weather data
   - Added `IWeatherService` dependency
   - Implemented coordinate lookup based on project location
   - Maps live API data to DTOs

## Quick Test Steps

### 1. Test Live Weather for Single Project

**API Endpoint:**
```http
GET /api/projects/weather/{projectId}
Authorization: Bearer {token}
```

**Expected Behavior:**
- Returns current/live weather data from OpenWeather API
- Data reflects current weather conditions (not 7 AM snapshot)
- Response should be similar to previous format but with live data

**Sample cURL:**
```bash
curl -X GET "https://your-api-url/api/projects/weather/{projectId}" \
     -H "Authorization: Bearer YOUR_TOKEN"
```

**What to Check:**
- ✅ Response is successful (200 OK)
- ✅ Weather data is returned with all fields populated
- ✅ `ReportDate` shows current date/time
- ✅ Temperature, humidity, wind speed are current values
- ✅ `IsFavorable` indicates if weather is good for construction
- ✅ `AlertMessage` appears if weather is unfavorable

### 2. Test All Projects Weather (Should Use Cached Data)

**API Endpoint:**
```http
GET /api/projects/weather/all
Authorization: Bearer {token}
```

**Expected Behavior:**
- Returns weather data from database (ProjectWeatherReport table)
- Fast response (no external API calls)
- Shows data from last background job run (7 AM)

### 3. Test Background Job (Should Still Work)

**API Endpoint:**
```http
POST /api/TestWeather/weather-run
```

**Expected Behavior:**
- Fetches weather for all active projects
- Saves data to ProjectWeatherReport table
- Creates quality issues if weather is unfavorable
- Sends notifications to project managers

**Check Database After:**
```sql
-- Check latest weather reports
SELECT TOP 10 
    r.ReportDate,
    p.ProjectCode,
    p.ProjectName,
    r.CurrentTemperature,
    r.WindGust,
    r.IsFavorable,
    r.QualityIssueCreated
FROM ProjectWeatherReport r
JOIN Projects p ON r.ProjectId = p.ProjectId
ORDER BY r.ReportDate DESC
```

### 4. Compare Live vs Cached Data

To see the difference between live and cached data:

1. Call `GET /api/projects/weather/all` - note the weather data (cached)
2. Call `GET /api/projects/weather/{projectId}` for one of the projects - note the weather data (live)
3. Compare the values - they may differ if weather has changed since 7 AM

## Coordinate Mapping

The system uses these coordinates based on project location:

| Location | City | Coordinates | Lat/Lon |
|----------|------|-------------|---------|
| KSA | Rabigh, Saudi Arabia | 22.80, 39.04 | Used for all KSA projects |
| UAE | Expo 2020 Dubai, UAE | 25.19, 55.27 | Used for all UAE projects |

## Weather Favorable Criteria

Weather is considered **unfavorable** if:
- Wind gust > 24 km/h

Weather is **favorable** if all conditions are met.

## Expected Response Format

```json
{
  "data": {
    "reportId": "guid",
    "projectId": "guid",
    "projectCode": "P001",
    "projectName": "Project Name",
    "reportDate": "2026-02-11T14:30:00Z",
    "currentTemperature": 25.5,
    "minTemperature": 20.0,
    "maxTemperature": 28.0,
    "humidity": 45.0,
    "precipitationProbability": 10.0,
    "precipitationAmount": 0.0,
    "windSpeed": 15.0,
    "windGust": 20.0,
    "windDirection": 180.0,
    "pressure": 1015.0,
    "solarRadiation": 0.0,
    "sunrise": "2026-02-11T06:30:00Z",
    "sunset": "2026-02-11T18:00:00Z",
    "moonrise": null,
    "moonset": null,
    "latitude": 22.80,
    "longitude": 39.04,
    "elevation": 0.0,
    "description": "clear sky",
    "isFavorable": true,
    "alertMessage": null,
    "qualityIssueCreated": false,
    "createdDate": "2026-02-11T14:30:00Z"
  },
  "isSuccess": true,
  "error": null
}
```

## Troubleshooting

### Issue: "Could not retrieve current weather data"

**Possible Causes:**
- Weather API key is invalid or expired
- Network connectivity issues
- Weather API rate limit exceeded
- Invalid coordinates

**Solution:**
- Check `appsettings.json` for `WeatherApi:ApiKey`
- Verify API key is valid at https://openweathermap.org/
- Check network connectivity
- Verify coordinates are valid

### Issue: "Project not found"

**Possible Causes:**
- Invalid project ID
- Project doesn't exist

**Solution:**
- Verify the project ID is correct
- Check the Projects table in the database

### Issue: Response is slow

**Possible Causes:**
- Weather API is slow to respond
- Network latency

**Solution:**
- This is expected for live data (typically 500ms-2s)
- Consider adding response caching if needed

## Performance Notes

| Endpoint | Data Source | Speed | API Calls |
|----------|-------------|-------|-----------|
| Single Project (`/{projectId}`) | Live API | ~500ms-2s | 1 per request |
| All Projects (`/all`) | Database | <100ms | 0 |
| Background Job | Live API | Varies | 1 per project |

## Next Steps

After testing, consider:

1. **Add Response Caching** - Cache live weather for 15-30 minutes to reduce API calls
2. **Add Fallback Logic** - If API fails, fall back to latest database record
3. **Monitor API Usage** - Track API calls to ensure within rate limits
4. **Add Health Checks** - Monitor weather API availability

## Questions or Issues?

If you encounter any issues:

1. Check the application logs for detailed error messages
2. Verify the weather API key is valid
3. Check network connectivity
4. Review the database to ensure projects have valid locations

## Summary

✅ Live weather data is now available when viewing individual projects  
✅ Background job continues to work for notifications and quality issues  
✅ All projects view uses cached data for performance  
✅ System provides best of both worlds: real-time data + automated monitoring
