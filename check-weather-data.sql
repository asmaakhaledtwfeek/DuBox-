-- Query to check all weather data for a specific project
-- Replace the ProjectId with your actual project ID

DECLARE @ProjectId UNIQUEIDENTIFIER = '8356A42E-D4B1-4321-8B17-08DE415C8695';
DECLARE @ProjectCode NVARCHAR(50) = '160';

-- ============================================
-- 1. Check Project Details
-- ============================================
PRINT '============================================'
PRINT '1. PROJECT INFORMATION'
PRINT '============================================'
PRINT ''

SELECT 
    ProjectId,
    ProjectCode,
    ProjectName,
    ClientName,
    Location,
    CASE Location 
        WHEN 0 THEN 'KSA (Rabigh: 22.80°N, 39.04°E)' 
        WHEN 1 THEN 'UAE (Expo 2020: 25.19°N, 55.27°E)' 
        ELSE 'Unknown' 
    END as LocationDescription,
    Status,
    CASE Status
        WHEN 1 THEN 'Active'
        ELSE 'Inactive'
    END as StatusDescription,
    ActualStartDate,
    CreatedDate
FROM Projects
WHERE ProjectId = @ProjectId OR ProjectCode = @ProjectCode

PRINT ''
PRINT '============================================'
PRINT '2. WEATHER REPORTS FOR THIS PROJECT'
PRINT '============================================'
PRINT ''

-- ============================================
-- 2. Check if weather reports exist
-- ============================================
IF EXISTS (SELECT 1 FROM ProjectWeatherReports WHERE ProjectId = @ProjectId)
BEGIN
    PRINT '✓ Weather reports found!'
    PRINT ''
    
    -- Show all weather data
    SELECT 
        ReportId,
        ProjectId,
        ReportDate,
        
        -- Temperature
        CurrentTemperature as [Current Temp (°C)],
        MinTemperature as [Min Temp (°C)],
        MaxTemperature as [Max Temp (°C)],
        
        -- Humidity & Precipitation
        Humidity as [Humidity (%)],
        PrecipitationProbability as [Precip Probability (%)],
        PrecipitationAmount as [Precip Amount (mm)],
        
        -- Wind
        WindSpeed as [Wind Speed (km/h)],
        WindGust as [Wind Gust (km/h)],
        WindDirection as [Wind Direction (°)],
        
        -- Pressure & Solar
        Pressure as [Pressure (hPa)],
        SolarRadiation as [Solar Radiation (wh/m²)],
        
        -- Sun Times
        CONVERT(VARCHAR(5), Sunrise, 108) as [Sunrise (HH:MM)],
        CONVERT(VARCHAR(5), Sunset, 108) as [Sunset (HH:MM)],
        
        -- Moon Times
        CONVERT(VARCHAR(5), Moonrise, 108) as [Moonrise (HH:MM)],
        CONVERT(VARCHAR(5), Moonset, 108) as [Moonset (HH:MM)],
        
        -- Location
        CONCAT(
            CAST(FLOOR(ABS(Latitude)) as VARCHAR(3)), '° ',
            CAST(FLOOR((ABS(Latitude) - FLOOR(ABS(Latitude))) * 60) as VARCHAR(2)), ''' ',
            CAST(FLOOR(((ABS(Latitude) - FLOOR(ABS(Latitude))) * 60 - FLOOR((ABS(Latitude) - FLOOR(ABS(Latitude))) * 60)) * 60) as VARCHAR(2)), '" ',
            CASE WHEN Latitude >= 0 THEN 'N' ELSE 'S' END, ' ',
            CAST(FLOOR(ABS(Longitude)) as VARCHAR(3)), '° ',
            CAST(FLOOR((ABS(Longitude) - FLOOR(ABS(Longitude))) * 60) as VARCHAR(2)), ''' ',
            CAST(FLOOR(((ABS(Longitude) - FLOOR(ABS(Longitude))) * 60 - FLOOR((ABS(Longitude) - FLOOR(ABS(Longitude))) * 60)) * 60) as VARCHAR(2)), '" ',
            CASE WHEN Longitude >= 0 THEN 'E' ELSE 'W' END
        ) as [Coordinates],
        Latitude as [Latitude (Decimal)],
        Longitude as [Longitude (Decimal)],
        Elevation as [Elevation (m)],
        
        -- Status
        Description,
        IsFavorable,
        CASE WHEN IsFavorable = 1 THEN 'Favorable' ELSE 'Unfavorable' END as [Weather Status],
        AlertMessage,
        QualityIssueCreated,
        CreatedDate
    FROM ProjectWeatherReports
    WHERE ProjectId = @ProjectId
    ORDER BY ReportDate DESC
    
    PRINT ''
    PRINT 'Weather data retrieved successfully!'
END
ELSE
BEGIN
    PRINT '✗ No weather reports found for this project.'
    PRINT ''
    PRINT 'Possible reasons:'
    PRINT '  1. WeatherMonitoringWorker has not run yet (scheduled for 7 AM KSA/UAE)'
    PRINT '  2. Project ActualStartDate is in the future'
    PRINT '  3. Project is not Active'
    PRINT '  4. Weather API call failed'
    PRINT ''
    PRINT 'To generate weather data:'
    PRINT '  - Wait for scheduled run at 7 AM local time, OR'
    PRINT '  - Manually trigger the WeatherMonitoringWorker, OR'
    PRINT '  - Use the TestWeatherController endpoint'
END

PRINT ''
PRINT '============================================'
PRINT '3. CHECK WHAT DATA IS MISSING'
PRINT '============================================'
PRINT ''

-- Check for NULL or zero values in important fields
SELECT 
    ReportDate,
    CASE WHEN CurrentTemperature IS NULL OR CurrentTemperature = 0 THEN '✗' ELSE '✓' END as [Has Temp],
    CASE WHEN Humidity IS NULL OR Humidity = 0 THEN '✗' ELSE '✓' END as [Has Humidity],
    CASE WHEN WindSpeed IS NULL THEN '✗' ELSE '✓' END as [Has Wind],
    CASE WHEN Pressure IS NULL OR Pressure = 0 THEN '✗' ELSE '✓' END as [Has Pressure],
    CASE WHEN SolarRadiation IS NULL THEN '✗' ELSE '✓' END as [Has Solar],
    CASE WHEN Sunrise IS NULL THEN '✗' ELSE '✓' END as [Has Sunrise],
    CASE WHEN Sunset IS NULL THEN '✗' ELSE '✓' END as [Has Sunset],
    CASE WHEN Latitude IS NULL OR Latitude = 0 THEN '✗' ELSE '✓' END as [Has Location]
FROM ProjectWeatherReports
WHERE ProjectId = @ProjectId
ORDER BY ReportDate DESC

PRINT ''
PRINT '============================================'
PRINT '4. LATEST WEATHER SUMMARY'
PRINT '============================================'
PRINT ''

-- Show a formatted summary of the latest weather
SELECT TOP 1
    CONCAT('Temperature: ', CurrentTemperature, '°C (', MinTemperature, '-', MaxTemperature, '°C)') as [Temperature],
    CONCAT('Humidity: ', Humidity, '%') as [Humidity],
    CONCAT('Wind: ', WindSpeed, ' km/h at ', WindDirection, '°') as [Wind],
    CONCAT('Pressure: ', Pressure, ' hPa') as [Pressure],
    CONCAT('Solar: ', ISNULL(CAST(SolarRadiation as VARCHAR), 'N/A'), ' wh/m²') as [Solar Radiation],
    CONCAT('Sunrise: ', CONVERT(VARCHAR(5), Sunrise, 108), ' | Sunset: ', CONVERT(VARCHAR(5), Sunset, 108)) as [Sun Times],
    CONCAT(
        CAST(FLOOR(ABS(Latitude)) as VARCHAR), '° ',
        CAST(FLOOR((ABS(Latitude) - FLOOR(ABS(Latitude))) * 60) as VARCHAR), ''' ',
        CASE WHEN Latitude >= 0 THEN 'N' ELSE 'S' END, ' ',
        CAST(FLOOR(ABS(Longitude)) as VARCHAR), '° ',
        CAST(FLOOR((ABS(Longitude) - FLOOR(ABS(Longitude))) * 60) as VARCHAR), ''' ',
        CASE WHEN Longitude >= 0 THEN 'E' ELSE 'W' END,
        ' / ', Elevation, 'm'
    ) as [Location],
    Description as [Condition],
    CASE WHEN IsFavorable = 1 THEN '✓ Favorable' ELSE '✗ Unfavorable' END as [Status]
FROM ProjectWeatherReports
WHERE ProjectId = @ProjectId
ORDER BY ReportDate DESC

PRINT ''
PRINT '============================================'
PRINT 'COMPLETE!'
PRINT '============================================'
PRINT ''
PRINT 'To view this data in the UI:'
PRINT '  1. Navigate to: http://localhost:4200/schedule'
PRINT '  2. Select project: RSG 17 Linear Buildings (Code: 160)'
PRINT '  3. Scroll down to the Weather section'
PRINT ''
PRINT 'Expected coordinates for UAE project:'
PRINT '  Expo 2020 Dubai: 25°11''24"N 55°16''12"E'
PRINT ''
