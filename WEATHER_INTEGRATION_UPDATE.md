# Weather Integration Update - Live Data for Project View

## Summary

Updated the weather integration to fetch **live weather data** when a user opens a project, while keeping the background job for notifications and quality issues.

## Changes Made

### 1. Modified `GetProjectWeatherReportQueryHandler.cs`

**Location:** `Dubox.Application\Features\Projects\Queries\GetProjectWeatherReportQueryHandler.cs`

**Changes:**
- Added `IWeatherService` dependency injection
- Changed from reading stored weather reports from the database to fetching **live weather data** directly from the weather API
- Implemented coordinate lookup based on project location (KSA or UAE)
- Maps live `WeatherAlertInfo` data to `ProjectWeatherReportDto`
- Calculates favorable/unfavorable conditions in real-time
- Generates alert messages on-the-fly

**Key Methods:**
- `GetCoordinatesForProject()` - Returns coordinates based on project location
  - KSA projects → Rabigh (22.80, 39.04)
  - UAE projects → Expo 2020 Dubai (25.19, 55.27)
- `BuildWeatherAlertMessage()` - Creates user-friendly weather alert messages

## Architecture

### Current Flow

```
User Opens Project
    ↓
Frontend calls GET /api/projects/weather/{projectId}
    ↓
GetProjectWeatherReportQueryHandler
    ↓
Fetches LIVE weather from OpenWeather API via IWeatherService
    ↓
Returns real-time weather data to frontend
```

### Background Job (Unchanged)

```
WeatherMonitoringWorker runs at 7 AM local time
    ↓
Fetches weather for all active projects
    ↓
Saves to ProjectWeatherReport table
    ↓
Creates quality issues if weather is unfavorable
    ↓
Sends notifications to project managers
```

## What Stays the Same

1. **Background Job (`WeatherMonitoringWorker`)**: Continues to run at 7 AM local time for each location (KSA and UAE)
2. **Database Storage**: Weather reports are still saved to `ProjectWeatherReport` table for historical tracking
3. **Quality Issues**: Quality issues are still created automatically when unfavorable weather is detected
4. **Notifications**: Real-time notifications are still sent to project managers via SignalR
5. **All Projects Endpoint**: `GET /api/projects/weather/all` still reads from the database (cached data) to avoid multiple API calls

## What Changed

1. **Single Project Endpoint**: `GET /api/projects/weather/{projectId}` now fetches **live weather data** from the API
2. **Real-time Updates**: Users always see current weather conditions when they open a project
3. **No Stale Data**: Weather data in the project view is never out of date

## Benefits

1. **Real-time Weather**: Users get current weather conditions, not data from the last scheduled run
2. **More Accurate**: Weather conditions can change throughout the day; live data reflects this
3. **Better Decision Making**: Project managers can make informed decisions based on current conditions
4. **Dual Purpose**: Background job focuses on notifications and quality tracking, while project view provides real-time data

## API Endpoints

### Single Project Weather (LIVE DATA)
- **Endpoint:** `GET /api/projects/weather/{projectId}`
- **Data Source:** Live API call to OpenWeather
- **Use Case:** When a user opens a specific project
- **Response Time:** Depends on weather API response (~500ms-2s)

### All Projects Weather (CACHED DATA)
- **Endpoint:** `GET /api/projects/weather/all`
- **Data Source:** Database (ProjectWeatherReport table)
- **Use Case:** Dashboard/list views showing multiple projects
- **Response Time:** Fast (database query)

## Technical Details

### Dependencies
- `IWeatherService` - Injected into query handler
- OpenWeather API - Used for live weather data
- Coordinates based on project location enum

### Data Mapping
- `WeatherAlertInfo` (from API) → `ProjectWeatherReportDto` (to frontend)
- All weather fields are mapped (temperature, humidity, wind, precipitation, etc.)
- Favorable/unfavorable status calculated using `IsFavorableForConstruction()` method

### Coordinates by Location
```csharp
ProjectLocationEnum.KSA → (22.80, 39.04) // Rabigh, Saudi Arabia
ProjectLocationEnum.UAE → (25.19, 55.27) // Expo 2020 Dubai, UAE
```

## Testing

To test the changes:

1. **Test Live Weather Fetch:**
   ```
   GET /api/projects/weather/{projectId}
   ```
   - Should return current weather data
   - Data should be different from what's in the database (if weather has changed)
   - Response should include all weather fields

2. **Test Background Job:**
   ```
   POST /api/TestWeather/weather-run
   ```
   - Should still save weather reports to database
   - Should create quality issues if weather is unfavorable
   - Should send notifications

3. **Test All Projects Endpoint:**
   ```
   GET /api/projects/weather/all
   ```
   - Should return cached data from database
   - Should be fast (no external API calls)

## Migration Notes

- No database schema changes required
- No frontend changes required (API contract remains the same)
- Existing weather reports in the database are preserved
- Background job continues to populate the database for historical tracking

## Performance Considerations

- **Single project view:** One API call per project view (acceptable for single project)
- **All projects view:** No API calls (uses cached data from database)
- **Rate limiting:** Weather API has rate limits; current implementation only makes API calls when users open individual projects
- **Caching option:** Could add response caching in the future if needed (e.g., cache for 15 minutes)

## Future Enhancements

Possible improvements for the future:

1. **Response Caching:** Cache live weather data for 15-30 minutes to reduce API calls
2. **Fallback to Database:** If weather API is unavailable, fall back to latest database record
3. **Location-specific Coordinates:** Allow projects to have specific coordinates instead of location-based defaults
4. **Weather History:** Add endpoint to view historical weather data from the database
5. **Weather Trends:** Show weather trends over time using historical data

## Conclusion

The weather integration now provides the best of both worlds:
- **Real-time data** when users need it (individual project views)
- **Automated monitoring** for proactive notifications and quality management
- **Efficient data access** for dashboard views using cached data

This approach ensures users always have current weather information while maintaining system performance and reliability.
