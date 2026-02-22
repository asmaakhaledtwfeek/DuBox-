using Dubox.Application.DTOs;
using Dubox.Application.Specifications;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dubox.Application.Features.Projects.Queries
{
    public class GetProjectWeatherReportQueryHandler : IRequestHandler<GetProjectWeatherReportQuery, Result<ProjectWeatherReportDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeatherService _weatherService;
        private const int CacheExpirationMinutes = 30;

        public GetProjectWeatherReportQueryHandler(
            IUnitOfWork unitOfWork,
            IWeatherService weatherService)
        {
            _unitOfWork = unitOfWork;
            _weatherService = weatherService;
        }

        public async Task<Result<ProjectWeatherReportDto>> Handle(
            GetProjectWeatherReportQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Get project to verify it exists and get location
            var project = await _unitOfWork.Repository<Project>()
                .GetByIdAsync(request.ProjectId, cancellationToken);

            if (project == null)
            {
                return Result.Failure<ProjectWeatherReportDto>("Project not found");
            }

            // 2. Get coordinates based on project location
            var (latitude, longitude) = _weatherService.GetCoordinatesForProject(project);

            // 3. Round coordinates to avoid minor differences (4 decimal places ≈ 11 meters accuracy)
            var roundedLat = Math.Round(latitude, 4);
            var roundedLon = Math.Round(longitude, 4);

            // 4. Check cache for existing weather data
            var cacheRepository = _unitOfWork.Repository<WeatherDataCache>();
            var cachedData =  cacheRepository.GetEntityWithSpec( new WeatherDataCashSpecification(roundedLon, roundedLat));
               

            WeatherAlertInfo weatherInfo;
            var cacheExpirationTime = DateTime.UtcNow.AddMinutes(-CacheExpirationMinutes);

            // 5. Check if cache is valid (exists and less than 30 minutes old)
            if (cachedData != null && cachedData.LastUpdated >= cacheExpirationTime)
            {
                // Use cached data
                weatherInfo = MapCacheToWeatherInfo(cachedData);
            }
            else
            {
                // 6. Fetch fresh data from weather service
                weatherInfo = await _weatherService.GetWeatherForecastByCoordinatesAsync(latitude, longitude);

                if (weatherInfo == null)
                {
                    return Result.Failure<ProjectWeatherReportDto>("Could not retrieve current weather data");
                }

                // 7. Update or insert cache
                if (cachedData != null)
                {
                    // Update existing cache entry
                    UpdateCacheFromWeatherInfo(cachedData, weatherInfo);
                    cachedData.LastUpdated = DateTime.UtcNow;
                     cacheRepository.Update(cachedData);
                }
                else
                {
                    // Insert new cache entry
                    var newCache = new WeatherDataCache
                    {
                        CacheId = Guid.NewGuid(),
                        Latitude = (decimal)roundedLat,
                        Longitude = (decimal)roundedLon,
                        LastUpdated = DateTime.UtcNow
                    };
                    UpdateCacheFromWeatherInfo(newCache, weatherInfo);
                    await cacheRepository.AddAsync(newCache, cancellationToken);
                }

                // Save changes to database
                await _unitOfWork.CompleteAsync(cancellationToken);
            }

            // 8. Check if weather is favorable for construction
            var isFavorable = weatherInfo.IsFavorableForConstruction();
            var alertMessage = isFavorable ? null : BuildWeatherAlertMessage(weatherInfo);

            // 9. Map weather data to DTO
            var dto = new ProjectWeatherReportDto
            {
                ReportId = Guid.NewGuid(), // Generate new ID for the response
                ProjectId = project.ProjectId,
                ProjectCode = project.ProjectCode,
                ProjectName = project.ProjectName,
                ReportDate = DateTime.UtcNow,
                CurrentTemperature = weatherInfo.CurrentTemp,
                MinTemperature = weatherInfo.MinTemp,
                MaxTemperature = weatherInfo.MaxTemp,
                Humidity = weatherInfo.Humidity,
                PrecipitationProbability = weatherInfo.PrecipitationProbability,
                PrecipitationAmount = weatherInfo.PrecipitationAmount,
                WindSpeed = weatherInfo.WindSpeed,
                WindGust = weatherInfo.WindGust,
                WindDirection = weatherInfo.WindDirection,
                Pressure = weatherInfo.Pressure,
                SolarRadiation = weatherInfo.SolarRadiation,
                Sunrise = weatherInfo.Sunrise,
                Sunset = weatherInfo.Sunset,
                Moonrise = weatherInfo.Moonrise,
                Moonset = weatherInfo.Moonset,
                Latitude = weatherInfo.Latitude,
                Longitude = weatherInfo.Longitude,
                Elevation = weatherInfo.Elevation,
                Description = weatherInfo.Description,
                IsFavorable = isFavorable,
                AlertMessage = alertMessage,
                QualityIssueCreated = false,
                CreatedDate = DateTime.UtcNow
            };

            return Result.Success(dto);
        }

        #region Private Helper Methods

        /// <summary>
        /// Maps cached weather data to WeatherAlertInfo object
        /// </summary>
        private WeatherAlertInfo MapCacheToWeatherInfo(WeatherDataCache cache)
        {
            return new WeatherAlertInfo
            {
                CurrentTemp =cache.CurrentTemperature,
                MinTemp = cache.MinTemperature,
                MaxTemp = cache.MaxTemperature,
                Humidity = cache.Humidity,
                PrecipitationProbability = cache.PrecipitationProbability,
                PrecipitationAmount = cache.PrecipitationAmount,
                WindSpeed = cache.WindSpeed,
                WindGust = cache.WindGust,
                WindDirection = (decimal)cache.WindDirection,
                Pressure = cache.Pressure,
                SolarRadiation = cache.SolarRadiation,
                Sunrise = cache.Sunrise,
                Sunset = cache.Sunset,
                Moonrise = cache.Moonrise,
                Moonset = cache.Moonset,
                Latitude = cache.Latitude,
                Longitude =cache.Longitude,
                Elevation = cache.Elevation,
                Description = cache.Description
            };
        }

        /// <summary>
        /// Updates cache entity with fresh weather data from service
        /// </summary>
        private void UpdateCacheFromWeatherInfo(WeatherDataCache cache, WeatherAlertInfo weatherInfo)
        {
            cache.CurrentTemperature = (decimal)weatherInfo.CurrentTemp;
            cache.MinTemperature = (decimal)weatherInfo.MinTemp;
            cache.MaxTemperature = (decimal)weatherInfo.MaxTemp;
            cache.Humidity = (decimal)weatherInfo.Humidity;
            cache.PrecipitationProbability = (decimal)weatherInfo.PrecipitationProbability;
            cache.PrecipitationAmount = (decimal)weatherInfo.PrecipitationAmount;
            cache.WindSpeed = (decimal)weatherInfo.WindSpeed;
            cache.WindGust = (decimal)weatherInfo.WindGust;
            cache.WindDirection = weatherInfo.WindDirection ;
            cache.Pressure = (decimal)weatherInfo.Pressure;
            cache.SolarRadiation = (decimal)weatherInfo.SolarRadiation;
            cache.Sunrise = weatherInfo.Sunrise;
            cache.Sunset = weatherInfo.Sunset;
            cache.Moonrise = weatherInfo.Moonrise;
            cache.Moonset = weatherInfo.Moonset;
            cache.Elevation = (decimal)weatherInfo.Elevation;
            cache.Description = weatherInfo.Description ?? string.Empty;
        }

        /// <summary>
        /// Builds alert message when weather is not favorable for construction
        /// </summary>
        private string BuildWeatherAlertMessage(WeatherAlertInfo weatherInfo)
        {
            return $"Temperature: {weatherInfo.MinTemp:F1}-{weatherInfo.MaxTemp:F1}°C, " +
                   $"Precipitation: {weatherInfo.PrecipitationProbability:F0}% / {weatherInfo.PrecipitationAmount:F1}mm, " +
                   $"Wind: {weatherInfo.WindGust:F0}km/h, " +
                   $"Conditions: {weatherInfo.Description}.";
        }

        #endregion
    }
}
