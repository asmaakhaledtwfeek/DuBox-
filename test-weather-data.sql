-- Weather Implementation Test Data Script
-- Run this script to create test projects and verify weather data

-- ============================================
-- PART 1: Create Test Projects
-- ============================================

PRINT 'Creating test projects for weather monitoring...'

-- Create Expo 2020 Dubai Test Project
IF NOT EXISTS (SELECT 1 FROM Projects WHERE ProjectCode = 'EXPO2020-TEST')
BEGIN
    INSERT INTO Projects (
        ProjectId, 
        ProjectCode, 
        ProjectName, 
        Location, -- 1 = UAE
        Status, -- 1 = Active
        IsActive, 
        ActualStartDate, 
        CreatedDate,
        ProjectDescription
    )
    VALUES (
        NEWID(), 
        'EXPO2020-TEST', 
        'Expo 2020 Dubai Weather Test Project',
        1, -- UAE
        1, -- Active
        1, -- IsActive
        GETDATE(), 
        GETDATE(),
        'Test project for Expo 2020 Dubai location (25.19°N, 55.27°E)'
    )
    PRINT '✓ Created Expo 2020 test project'
END
ELSE
    PRINT '○ Expo 2020 test project already exists'

-- Create Rabigh KSA Test Project  
IF NOT EXISTS (SELECT 1 FROM Projects WHERE ProjectCode = 'RABIGH-TEST')
BEGIN
    INSERT INTO Projects (
        ProjectId, 
        ProjectCode, 
        ProjectName, 
        Location, -- 0 = KSA
        Status, -- 1 = Active
        IsActive, 
        ActualStartDate, 
        CreatedDate,
        ProjectDescription
    )
    VALUES (
        NEWID(), 
        'RABIGH-TEST', 
        'Rabigh Construction Weather Test Project',
        0, -- KSA
        1, -- Active
        1, -- IsActive
        GETDATE(), 
        GETDATE(),
        'Test project for Rabigh, Saudi Arabia location (22.80°N, 39.04°E)'
    )
    PRINT '✓ Created Rabigh test project'
END
ELSE
    PRINT '○ Rabigh test project already exists'

PRINT ''
PRINT '============================================'
PRINT 'Test projects created successfully!'
PRINT '============================================'
PRINT ''

-- ============================================
-- PART 2: Verify New Weather Columns
-- ============================================

PRINT 'Verifying weather report table structure...'
PRINT ''

SELECT 
    COLUMN_NAME as [Column Name],
    DATA_TYPE as [Data Type],
    IS_NULLABLE as [Nullable]
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ProjectWeatherReports'
    AND COLUMN_NAME IN (
        'CurrentTemperature',
        'Humidity', 
        'WindSpeed',
        'WindDirection',
        'Pressure',
        'Sunrise',
        'Sunset',
        'Moonrise',
        'Moonset',
        'Latitude',
        'Longitude',
        'Elevation'
    )
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT 'Note: You should see 12 new columns listed above'
PRINT ''

-- ============================================
-- PART 3: Check Existing Weather Data
-- ============================================

PRINT '============================================'
PRINT 'Checking existing weather reports...'
PRINT '============================================'
PRINT ''

IF EXISTS (SELECT 1 FROM ProjectWeatherReports)
BEGIN
    SELECT TOP 10
        p.ProjectCode,
        p.ProjectName,
        w.ReportDate,
        w.CurrentTemperature as [Temp °C],
        w.Humidity as [Humidity %],
        w.WindSpeed as [Wind km/h],
        w.Pressure as [Pressure hPa],
        w.Latitude,
        w.Longitude,
        w.Description,
        w.IsFavorable
    FROM ProjectWeatherReports w
    INNER JOIN Projects p ON w.ProjectId = p.ProjectId
    ORDER BY w.ReportDate DESC

    PRINT ''
    PRINT '✓ Weather reports found'
END
ELSE
BEGIN
    PRINT 'ℹ No weather reports yet. Run the WeatherMonitoringWorker to generate data.'
    PRINT '  The worker runs automatically at 7 AM (KSA/UAE time)'
END

PRINT ''

-- ============================================
-- PART 4: Show Test Projects
-- ============================================

PRINT '============================================'
PRINT 'Test Projects Available:'
PRINT '============================================'
PRINT ''

SELECT 
    ProjectId,
    ProjectCode,
    ProjectName,
    CASE Location 
        WHEN 0 THEN 'KSA' 
        WHEN 1 THEN 'UAE' 
        ELSE 'Unknown' 
    END as Location,
    CASE Status
        WHEN 1 THEN 'Active'
        ELSE 'Inactive'
    END as Status,
    ActualStartDate,
    CreatedDate
FROM Projects
WHERE ProjectCode IN ('EXPO2020-TEST', 'RABIGH-TEST')
    OR ProjectCode LIKE '%EXPO%'
    OR ProjectCode LIKE '%RABIGH%'
ORDER BY CreatedDate DESC

PRINT ''

-- ============================================
-- PART 5: Query to Check Weather for Test Projects
-- ============================================

PRINT '============================================'
PRINT 'Weather Data for Test Projects:'
PRINT '============================================'
PRINT ''

SELECT 
    p.ProjectCode,
    w.ReportDate,
    w.CurrentTemperature,
    w.MinTemperature,
    w.MaxTemperature,
    w.Humidity,
    w.WindSpeed,
    w.WindDirection,
    w.Pressure,
    CONVERT(VARCHAR(5), w.Sunrise, 108) as Sunrise,
    CONVERT(VARCHAR(5), w.Sunset, 108) as Sunset,
    CONCAT(
        CAST(w.Latitude as VARCHAR(10)), '°N, ',
        CAST(w.Longitude as VARCHAR(10)), '°E'
    ) as Coordinates,
    w.Elevation,
    w.Description,
    w.IsFavorable,
    w.CreatedDate
FROM ProjectWeatherReports w
INNER JOIN Projects p ON w.ProjectId = p.ProjectId
WHERE p.ProjectCode IN ('EXPO2020-TEST', 'RABIGH-TEST')
    OR p.ProjectCode LIKE '%EXPO%'
    OR p.ProjectCode LIKE '%RABIGH%'
ORDER BY w.ReportDate DESC

IF @@ROWCOUNT = 0
BEGIN
    PRINT 'ℹ No weather data for test projects yet.'
    PRINT ''
    PRINT 'To generate weather data:'
    PRINT '  1. Ensure backend API is running'
    PRINT '  2. Wait for scheduled run at 7 AM (KSA/UAE time), OR'
    PRINT '  3. Use TestWeatherController to trigger manually'
END

PRINT ''
PRINT '============================================'
PRINT 'Testing Script Complete!'
PRINT '============================================'
PRINT ''
PRINT 'Next Steps:'
PRINT '  1. Run the backend API: dotnet run --project Dubox.Api'
PRINT '  2. Run the frontend: npm start (in dubox-frontend folder)'
PRINT '  3. Navigate to: http://localhost:4200/schedule'
PRINT '  4. Select a test project to view weather data'
PRINT ''
PRINT 'Expected Weather Coordinates:'
PRINT '  • EXPO projects: 25.19°N, 55.27°E (Dubai, UAE)'
PRINT '  • RABIGH projects: 22.80°N, 39.04°E (Rabigh, KSA)'
PRINT ''
