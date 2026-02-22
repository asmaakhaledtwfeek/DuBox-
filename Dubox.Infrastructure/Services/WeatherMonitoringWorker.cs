using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dubox.Infrastructure.Services
{
    public class WeatherMonitoringWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherMonitoringWorker> _logger;
        private const int ScheduledHour = 7; // 7 AM local time

        public WeatherMonitoringWorker(
            IServiceProvider serviceProvider,
            ILogger<WeatherMonitoringWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather Monitoring Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Calculate next run time (7 AM in project locations)
                    var delayUntilNextRun = CalculateDelayUntilNextRun();
                    
                    _logger.LogInformation("Next weather check scheduled in {hours} hours and {minutes} minutes", 
                        delayUntilNextRun.Hours, delayUntilNextRun.Minutes);

                    // Wait until the scheduled time
                    await Task.Delay(delayUntilNextRun, stoppingToken);

                    // Check weather for all active projects
                    await CheckWeatherForActiveProjectsAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Weather Monitoring Worker is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking weather for projects");
                    
                    // Wait 1 hour before retrying on error
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }

            _logger.LogInformation("Weather Monitoring Worker stopped");
        }

        private TimeSpan CalculateDelayUntilNextRun()
        {
            var utcNow = DateTime.UtcNow;
            var nextRunTimes = new List<DateTime>();
            
            // Get timezones for both locations
            var ksaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"); // UTC+3
            var uaeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"); // UTC+4
            
            // Calculate next 7 AM KSA time
            var ksaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, ksaTimeZone);
            var nextKsaRun = ksaNow.Date.AddHours(ScheduledHour);
            
            // If we're past 7 AM today in KSA, schedule for tomorrow
            if (ksaNow.Hour >= ScheduledHour)
            {
                nextKsaRun = nextKsaRun.AddDays(1);
            }
            
            // Convert KSA time back to UTC
            var nextKsaRunUtc = TimeZoneInfo.ConvertTimeToUtc(nextKsaRun, ksaTimeZone);
            nextRunTimes.Add(nextKsaRunUtc);
            
            // Calculate next 7 AM UAE time
            var uaeNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, uaeTimeZone);
            var nextUaeRun = uaeNow.Date.AddHours(ScheduledHour);
            
            // If we're past 7 AM today in UAE, schedule for tomorrow
            if (uaeNow.Hour >= ScheduledHour)
            {
                nextUaeRun = nextUaeRun.AddDays(1);
            }
            
            // Convert UAE time back to UTC
            var nextUaeRunUtc = TimeZoneInfo.ConvertTimeToUtc(nextUaeRun, uaeTimeZone);
            nextRunTimes.Add(nextUaeRunUtc);
            
            // Get the earliest next run time
            var nextRunUtc = nextRunTimes.Min();
            
            // Calculate delay
            var delay = nextRunUtc - utcNow;
            
            // Ensure minimum delay of 1 minute
            if (delay.TotalMinutes < 1)
            {
                delay = TimeSpan.FromMinutes(1);
            }
            
            // Log which location's schedule we're following
            var location = nextRunUtc == nextKsaRunUtc ? "KSA" : "UAE";
            var localTime = location == "KSA" 
                ? TimeZoneInfo.ConvertTimeFromUtc(nextRunUtc, ksaTimeZone)
                : TimeZoneInfo.ConvertTimeFromUtc(nextRunUtc, uaeTimeZone);
            
            _logger.LogInformation(
                "Next weather check scheduled for {location} at {localTime} local time (UTC: {utcTime})",
                location,
                localTime.ToString("yyyy-MM-dd HH:mm:ss"),
                nextRunUtc.ToString("yyyy-MM-dd HH:mm:ss"));
            
            return delay;
        }

        private async Task CheckWeatherForActiveProjectsAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
            var notificationHubService = scope.ServiceProvider.GetRequiredService<INotificationHubService>();

            try
            {
                var utcNow = DateTime.UtcNow;
                var ksaTimeZone = GetTimeZoneForLocation(ProjectLocationEnum.KSA);
                var uaeTimeZone = GetTimeZoneForLocation(ProjectLocationEnum.UAE);
                var ksaLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, ksaTimeZone);
                var uaeLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, uaeTimeZone);
                
                _logger.LogInformation(
                    "Starting weather check - UTC: {utcTime}, KSA: {ksaTime}, UAE: {uaeTime}", 
                    utcNow.ToString("yyyy-MM-dd HH:mm:ss"), 
                    ksaLocalTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    uaeLocalTime.ToString("yyyy-MM-dd HH:mm:ss"));

                // Determine which location(s) to check based on current time
                var locationsToCheck = new List<ProjectLocationEnum>();
                
                // Check if it's 7 AM in KSA (within 1 hour window)
                if (ksaLocalTime.Hour >= ScheduledHour - 1 && ksaLocalTime.Hour <= ScheduledHour)
                {
                    locationsToCheck.Add(ProjectLocationEnum.KSA);
                    _logger.LogInformation("Checking KSA projects (Local time: {time})", ksaLocalTime.ToString("HH:mm:ss"));
                }
                
                // Check if it's 7 AM in UAE (within 1 hour window)
                if (uaeLocalTime.Hour >= ScheduledHour - 1 && uaeLocalTime.Hour <= ScheduledHour)
                {
                    locationsToCheck.Add(ProjectLocationEnum.UAE);
                    _logger.LogInformation("Checking UAE projects (Local time: {time})", uaeLocalTime.ToString("HH:mm:ss"));
                }

                // If no specific location matches, check all locations (fallback)
                if (locationsToCheck.Count == 0)
                {
                    _logger.LogWarning("No location matched scheduled time, checking all locations");
                    locationsToCheck.Add(ProjectLocationEnum.KSA);
                    locationsToCheck.Add(ProjectLocationEnum.UAE);
                }

                // Get all active projects where actual start date <= today
                var today = DateTime.Today;
                var activeProjects = (await unitOfWork.Repository<Project>()
                    .FindAsync(p => p.IsActive && 
                                   p.Status == ProjectStatusEnum.Active &&
                                   p.ActualStartDate.HasValue && 
                                   p.ActualStartDate.Value.Date <= today &&
                                   locationsToCheck.Contains(p.Location), 
                              cancellationToken))
                    .ToList();

                _logger.LogInformation("Found {count} active projects to check in {locations}", 
                    activeProjects.Count, 
                    string.Join(", ", locationsToCheck));

                foreach (var project in activeProjects)
                {
                    try
                    {
                        var projectTimeZone = GetTimeZoneForLocation(project.Location);
                        var projectLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, projectTimeZone);
                        
                        _logger.LogInformation(
                            "Checking weather for project {projectCode} in {location} (Local time: {localTime})", 
                            project.ProjectCode, 
                            project.Location,
                            projectLocalTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        
                        await CheckProjectWeatherAsync(project, unitOfWork, weatherService, notificationHubService, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error checking weather for project {projectCode}", project.ProjectCode);
                    }
                }

                _logger.LogInformation("Completed weather check for {locations} projects", 
                    string.Join(", ", locationsToCheck));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckWeatherForActiveProjectsAsync");
                throw;
            }
        }

        private TimeZoneInfo GetTimeZoneForLocation(ProjectLocationEnum location)
        {
            return location switch
            {
                ProjectLocationEnum.KSA => TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"), // UTC+3
                ProjectLocationEnum.UAE => TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"), // UTC+4
                _ => TimeZoneInfo.Utc
            };
        }

        private async Task CheckProjectWeatherAsync(
            Project project,
            IUnitOfWork unitOfWork,
            IWeatherService weatherService,
            INotificationHubService notificationHubService,
            CancellationToken cancellationToken)
        {
            // Get coordinates based on project location and specific project details
            var (latitude, longitude) = weatherService.GetCoordinatesForProject(project);
            
            _logger.LogInformation("Checking weather for project {projectCode} at coordinates ({lat}, {lon})", 
                project.ProjectCode, latitude, longitude);

            // Get weather forecast using coordinates
            var weatherInfo = await weatherService.GetWeatherForecastByCoordinatesAsync(latitude, longitude);

            if (weatherInfo == null)
            {
                _logger.LogWarning("Could not retrieve weather data for project {projectCode}", project.ProjectCode);
                return;
            }

            _logger.LogInformation(
                "Weather for {projectCode}: Current: {current}°C, Temp {min}-{max}°C, Humidity: {humidity}%, POP: {pop}%, Precip: {precip}mm, Wind: {wind}km/h, Pressure: {pressure}hPa, Desc: {desc}",
                project.ProjectCode, weatherInfo.CurrentTemp, weatherInfo.MinTemp, weatherInfo.MaxTemp,
                weatherInfo.Humidity, weatherInfo.PrecipitationProbability, weatherInfo.PrecipitationAmount,
                weatherInfo.WindSpeed, weatherInfo.Pressure, weatherInfo.Description);

            

            // Check if weather is unfavorable for construction
            if (!weatherInfo.IsFavorableForConstruction())
            {
                _logger.LogWarning("Unfavorable weather detected for project {projectCode}", project.ProjectCode);
                
                await CreateWeatherQualityIssueAsync(project, weatherInfo, unitOfWork, cancellationToken);
                await SendWeatherNotificationsAsync(project, weatherInfo, unitOfWork, notificationHubService, cancellationToken);
            }
            else
            {
                _logger.LogInformation("Weather is favorable for project {projectCode}", project.ProjectCode);
            }
        }

        //private async Task SaveWeatherReportAsync(
        //    Project project,
        //    WeatherAlertInfo weatherInfo,
        //    IUnitOfWork unitOfWork,
        //    CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var today = DateTime.UtcNow.Date;

        //        // Check if we already have a report for today
        //        var existingReport = (await unitOfWork.Repository<WeatherDataCache>()
        //            .FindAsync(r => r.ProjectId == project.ProjectId && 
        //                           r.ReportDate.Date == today, 
        //                      cancellationToken))
        //            .FirstOrDefault();

        //        var isFavorable = weatherInfo.IsFavorableForConstruction();
        //        var alertMessage = isFavorable ? null : BuildWeatherSummary(weatherInfo);

        //        if (existingReport != null)
        //        {
        //            // Update existing report
        //            existingReport.CurrentTemperature = weatherInfo.CurrentTemp;
        //            existingReport.MinTemperature = weatherInfo.MinTemp;
        //            existingReport.MaxTemperature = weatherInfo.MaxTemp;
        //            existingReport.Humidity = weatherInfo.Humidity;
        //            existingReport.PrecipitationProbability = weatherInfo.PrecipitationProbability;
        //            existingReport.PrecipitationAmount = weatherInfo.PrecipitationAmount;
        //            existingReport.WindSpeed = weatherInfo.WindSpeed;
        //            existingReport.WindGust = weatherInfo.WindGust;
        //            existingReport.WindDirection = weatherInfo.WindDirection;
        //            existingReport.Pressure = weatherInfo.Pressure;
        //            existingReport.SolarRadiation = weatherInfo.SolarRadiation;
        //            existingReport.Sunrise = weatherInfo.Sunrise;
        //            existingReport.Sunset = weatherInfo.Sunset;
        //            existingReport.Moonrise = weatherInfo.Moonrise;
        //            existingReport.Moonset = weatherInfo.Moonset;
        //            existingReport.Latitude = weatherInfo.Latitude;
        //            existingReport.Longitude = weatherInfo.Longitude;
        //            existingReport.Elevation = weatherInfo.Elevation;
        //            existingReport.Description = weatherInfo.Description;
        //            existingReport.IsFavorable = isFavorable;
        //            existingReport.AlertMessage = alertMessage;

        //            unitOfWork.Repository<WeatherDataCache>().Update(existingReport);
        //            _logger.LogInformation("Updated weather report for project {projectCode}", project.ProjectCode);
        //        }
        //        else
        //        {
        //            // Create new report
        //            var weatherReport = new WeatherDataCache
        //            {
        //                ReportId = Guid.NewGuid(),
                      
        //                LastUpdated = today,
        //                CurrentTemperature = weatherInfo.CurrentTemp,
        //                MinTemperature = weatherInfo.MinTemp,
        //                MaxTemperature = weatherInfo.MaxTemp,
        //                Humidity = weatherInfo.Humidity,
        //                PrecipitationProbability = weatherInfo.PrecipitationProbability,
        //                PrecipitationAmount = weatherInfo.PrecipitationAmount,
        //                WindSpeed = weatherInfo.WindSpeed,
        //                WindGust = weatherInfo.WindGust,
        //                WindDirection = weatherInfo.WindDirection,
        //                Pressure = weatherInfo.Pressure,
        //                SolarRadiation = weatherInfo.SolarRadiation,
        //                Sunrise = weatherInfo.Sunrise,
        //                Sunset = weatherInfo.Sunset,
        //                Moonrise = weatherInfo.Moonrise,
        //                Moonset = weatherInfo.Moonset,
        //                Latitude = weatherInfo.Latitude,
        //                Longitude = weatherInfo.Longitude,
        //                Elevation = weatherInfo.Elevation,
        //                Description = weatherInfo.Description,
        //                IsFavorable = isFavorable,
        //                AlertMessage = alertMessage,
        //                QualityIssueCreated = false,
        //                CreatedDate = DateTime.UtcNow
        //            };

        //            await unitOfWork.Repository<WeatherDataCache>().AddAsync(weatherReport, cancellationToken);
        //            _logger.LogInformation("Created weather report for project {projectCode}", project.ProjectCode);
        //        }

        //        await unitOfWork.CompleteAsync(cancellationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving weather report for project {projectCode}", project.ProjectCode);
        //    }
        //}

        private async Task CreateWeatherQualityIssueAsync(
            Project project,
            WeatherAlertInfo weatherInfo,
            IUnitOfWork unitOfWork,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get QC team
                var qcTeam = (await unitOfWork.Repository<Team>()
                            .FindAsync(t => t.IsActive &&
                              (
                               (t.TeamCode != null && (t.TeamCode.ToLower() == "qc" || t.TeamCode.ToLower() == "qc-team")) ||
                               (t.TeamName != null && (t.TeamName.ToLower().Contains("qc") || t.TeamName.ToLower().Contains("quality control")))
                              ), cancellationToken))
                            .FirstOrDefault();
                var assignedToTeamId = qcTeam?.TeamId;
                if (qcTeam == null)
                {
                    _logger.LogWarning("QC team not found, quality issue will be created without team assignment");
                }

                // Generate issue number
                var issueCountInProject = (await unitOfWork.Repository<QualityIssue>()
                    .FindAsync(qi => qi.ProjectId == project.ProjectId || qi.Box.ProjectId==project.ProjectId, cancellationToken))
                    .Count();
                var issueNumber = (issueCountInProject + 1).ToString("D5");

                // Create quality issue
                var qualityIssue = new QualityIssue
                {
                    IssueId = Guid.NewGuid(),
                    ProjectId = project.ProjectId,
                    BoxId = null, // Weather issue is project-level, not box-specific
                    IssueNumber = issueNumber,
                    IssueDate = DateTime.UtcNow,
                    IssueType = IssueTypeEnum.Observation,
                    Severity = SeverityEnum.Major,
                    IssueDescription = BuildWeatherIssueDescription(weatherInfo),
                    ReportedBy = "Weather Monitoring System",
                    AssignedToTeamId = qcTeam?.TeamId,
                    AssignedToMemberId = null,
                    DueDate = DateTime.UtcNow.AddDays(1),
                    Status = QualityIssueStatusEnum.Open,
                    NCR = NCRTypeEnum.Internal,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = null // System-generated
                };

                await unitOfWork.Repository<QualityIssue>().AddAsync(qualityIssue, cancellationToken);
                
                await unitOfWork.CompleteAsync(cancellationToken);

                _logger.LogInformation("Created quality issue {issueNumber} for project {projectCode}", 
                    issueNumber, project.ProjectCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quality issue for project {projectCode}", project.ProjectCode);
            }
        }

        private async Task SendWeatherNotificationsAsync(
            Project project,
            WeatherAlertInfo weatherInfo,
            IUnitOfWork unitOfWork,
            INotificationHubService notificationHubService,
            CancellationToken cancellationToken)
        {
            try
            {
                var recipientUserIds = new List<Guid>();

                // Add project manager
                if (project.ProjectMangerId.HasValue)
                {
                    recipientUserIds.Add(project.ProjectMangerId.Value);
                }

                // Add created by user if different from project manager
                if (project.CreatedBy.HasValue && project.CreatedBy.Value != project.ProjectMangerId)
                {
                    recipientUserIds.Add(project.CreatedBy.Value);
                }

                var notificationTitle = "Weather Alert";
                var notificationMessage = $"Unfavorable weather conditions detected for project '{project.ProjectName}' ({project.ProjectCode}). " +
                    BuildWeatherSummary(weatherInfo) + " Please review and take necessary precautions.";

                foreach (var userId in recipientUserIds.Distinct())
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        RecipientUserId = userId,
                        Title = notificationTitle,
                        Message = notificationMessage,
                        NotificationType = "WeatherAlert",
                        Priority = "High",
                        IsRead = false,
                        CreatedDate = DateTime.UtcNow
                    };

                    await unitOfWork.Repository<Notification>().AddAsync(notification, cancellationToken);
                    
                    // Send real-time notification via SignalR
                    try
                    {
                        await notificationHubService.SendNotificationToUserAsync(userId, new
                        {
                            notificationId = notification.NotificationId,
                            title = notification.Title,
                            message = notification.Message,
                            type = notification.NotificationType,
                            createdAt = notification.CreatedDate
                        });

                        // Update unread count
                        var unreadCount = (await unitOfWork.Repository<Notification>()
                            .FindAsync(n => n.RecipientUserId == userId && !n.IsRead, cancellationToken))
                            .Count();
                        
                        await notificationHubService.SendNotificationCountUpdateAsync(userId, unreadCount);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send real-time notification to user {userId}", userId);
                    }
                }

                await unitOfWork.CompleteAsync(cancellationToken);
                
                _logger.LogInformation("Sent weather notifications for project {projectCode} to {count} users", 
                    project.ProjectCode, recipientUserIds.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notifications for project {projectCode}", project.ProjectCode);
            }
        }

      

        public async Task RunNowAsync()
        {
            await CheckWeatherForActiveProjectsAsync(CancellationToken.None);
        }

        private string BuildWeatherIssueDescription(WeatherAlertInfo weatherInfo)
        {
            var issues = new List<string>();

            if (weatherInfo.MinTemp < 19 || weatherInfo.MaxTemp > 26)
            {
                issues.Add($"Temperature outside optimal range: {weatherInfo.MinTemp:F1}-{weatherInfo.MaxTemp:F1}°C (optimal: 19-26°C)");
            }

            if (weatherInfo.PrecipitationProbability > 0)
            {
                issues.Add($"Precipitation probability: {weatherInfo.PrecipitationProbability:F0}%");
            }

            if (weatherInfo.PrecipitationAmount > 0)
            {
                issues.Add($"Expected precipitation: {weatherInfo.PrecipitationAmount:F1}mm");
            }

            if (weatherInfo.WindGust > 24)
            {
                issues.Add($"High wind gusts: {weatherInfo.WindGust:F0}km/h (threshold: 24km/h)");
            }

            var description = $"Unfavorable weather conditions detected:\n\n" +
                string.Join("\n", issues) +
                $"\n\nWeather description: {weatherInfo.Description}\n" +
                $"Forecast date: {weatherInfo.ForecastDate:yyyy-MM-dd HH:mm}\n\n" +
                "Please review site conditions and implement appropriate safety measures.";

            return description;
        }

        private string BuildWeatherSummary(WeatherAlertInfo weatherInfo)
        {
            return $"Temperature: {weatherInfo.MinTemp:F1}-{weatherInfo.MaxTemp:F1}°C, " +
                   $"Precipitation: {weatherInfo.PrecipitationProbability:F0}% / {weatherInfo.PrecipitationAmount:F1}mm, " +
                   $"Wind: {weatherInfo.WindGust:F0}km/h, " +
                   $"Conditions: {weatherInfo.Description}.";
        }
      
    }
}
