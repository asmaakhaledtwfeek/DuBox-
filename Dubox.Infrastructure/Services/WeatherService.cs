using Dubox.Application.DTOs;
using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Dubox.Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _elevationHttpClient;
        private readonly string _apiKey;
        private readonly bool _useOneCallApi;
        private readonly ILogger<WeatherService> _logger;
        private const string OPEN_ELEVATION_API = "https://api.open-elevation.com/api/v1";

        public WeatherService(
            HttpClient httpClient, 
            IConfiguration configuration,
            ILogger<WeatherService> logger,
            IHttpClientFactory httpClientFactory = null)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? "YOUR_API_KEY";
            _useOneCallApi = configuration.GetValue<bool>("WeatherApi:UseOneCallApi", false);
            _logger = logger;
            
            // Create separate client for elevation API to avoid conflicts
            _elevationHttpClient = httpClientFactory?.CreateClient() ?? new HttpClient();
            _elevationHttpClient.BaseAddress = new Uri(OPEN_ELEVATION_API);
        }

        public async Task<WeatherAlertInfo> GetWeatherForecastAsync(string city)
        {
            try
            {
                // Get current weather
                var currentResponse = await _httpClient.GetFromJsonAsync<CurrentWeatherRoot>(
                    $"weather?q={city}&appid={_apiKey}&units=metric");

                if (currentResponse == null)
                {
                    return null;
                }

                // Get forecast for precipitation probability
                var forecastResponse = await _httpClient.GetFromJsonAsync<ForecastRoot>(
                    $"forecast?q={city}&appid={_apiKey}&units=metric");

                var forecastData = forecastResponse?.List?.FirstOrDefault();

                return BuildWeatherAlertInfo(currentResponse, forecastData);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return null;
            }
        }

        public async Task<WeatherAlertInfo> GetWeatherForecastByCoordinatesAsync(decimal latitude, decimal longitude)
        {
            try
            {
                // Use One Call API 3.0 if configured, otherwise fall back to standard API
                if (_useOneCallApi)
                {
                    return await GetWeatherFromOneCallApiAsync(latitude, longitude);
                }
                else
                {
                    return await GetWeatherFromStandardApiAsync(latitude, longitude);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching weather data for coordinates ({lat}, {lon})", latitude, longitude);
                return null;
            }
        }

        private async Task<WeatherAlertInfo> GetWeatherFromStandardApiAsync(decimal latitude, decimal longitude)
        {
            // Get current weather by coordinates
            var currentResponse = await _httpClient.GetFromJsonAsync<CurrentWeatherRoot>(
                $"weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            if (currentResponse == null)
            {
                return null;
            }

            // Get forecast for precipitation probability
            var forecastResponse = await _httpClient.GetFromJsonAsync<ForecastRoot>(
                $"forecast?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            var forecastData = forecastResponse?.List?.FirstOrDefault();

            return BuildWeatherAlertInfo(currentResponse, forecastData, latitude, longitude);
        }

        private async Task<WeatherAlertInfo> GetWeatherFromOneCallApiAsync(decimal latitude, decimal longitude)
        {
            try
            {
                // Call One Call API 3.0 for comprehensive weather data
                var oneCallResponse = await _httpClient.GetFromJsonAsync<OneCallApiResponse>(
                    $"data/3.0/onecall?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&exclude=minutely,hourly,alerts");

                if (oneCallResponse?.Current == null)
                {
                    _logger?.LogWarning("One Call API returned null response, falling back to standard API");
                    return await GetWeatherFromStandardApiAsync(latitude, longitude);
                }

                // Get elevation data from separate API
                var elevation = await GetElevationAsync(latitude, longitude);

                // Build comprehensive weather info from One Call API
                return BuildWeatherAlertInfoFromOneCall(oneCallResponse, latitude, longitude, elevation);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error with One Call API, falling back to standard API");
                // Fall back to standard API if One Call fails
                return await GetWeatherFromStandardApiAsync(latitude, longitude);
            }
        }

    private WeatherAlertInfo BuildWeatherAlertInfo(CurrentWeatherRoot current, ForecastItem forecast, decimal? lat = null, decimal? lon = null)
    {
        if (current == null)
        {
            return null;
        }

        var weatherInfo = new WeatherAlertInfo
        {
            // Temperature (°C)
            CurrentTemp = current.Main.Temp,
            MinTemp = current.Main.TempMin,
            MaxTemp = current.Main.TempMax,
            
            // Humidity (%)
            Humidity = current.Main.Humidity,
            
            // Precipitation
            PrecipitationProbability = forecast?.Pop * 100 ?? 0, // Convert to percentage (%)
            PrecipitationAmount = forecast?.Rain?.ThreeHour ?? 0, // in mm
            
            // Wind (km/h and degrees)
            WindSpeed = current.Wind?.Speed ?? 0,
            WindGust = current.Wind?.Gust ?? current.Wind?.Speed ?? 0,
            WindDirection = current.Wind?.Deg ?? 0,
            
            // Atmospheric Pressure (hPa)
            Pressure = current.Main.Pressure,
            
            // Solar Radiation (wh/m²)
            // Note: OpenWeather free tier doesn't provide solar radiation data
            // Would require One Call API 3.0 subscription or separate solar API
            SolarRadiation = 0,
            
            // Sun Times (UTC)
            Sunrise = current.Sys?.Sunrise > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(current.Sys.Sunrise).DateTime 
                : null,
            Sunset = current.Sys?.Sunset > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(current.Sys.Sunset).DateTime 
                : null,
            
            // Moon Times (UTC)
            // Note: OpenWeather API doesn't provide moon data in the standard API
            // Would require One Call API 3.0 or separate astronomy API (e.g., astronomyapi.com)
            // For now, setting as null
            Moonrise = null,
            Moonset = null,
            
            // Location
            Latitude = lat ?? current.Coord?.Lat ?? 0,
            Longitude = lon ?? current.Coord?.Lon ?? 0,
            
            // Elevation (meters)
            // Note: OpenWeather doesn't provide elevation data
            // Would need to use a separate elevation API (e.g., Open-Elevation API)
            Elevation = 0,
            
            // Weather Description
            Description = current.Weather?.FirstOrDefault()?.Description ?? "N/A",
            
            // Forecast/Report Date
            ForecastDate = DateTime.UtcNow
        };

        return weatherInfo;
    }

    private WeatherAlertInfo BuildWeatherAlertInfoFromOneCall(
        OneCallApiResponse oneCall, 
        decimal latitude, 
        decimal longitude, 
        decimal elevation)
    {
        var current = oneCall.Current;
        var today = oneCall.Daily?.FirstOrDefault();

        if (current == null)
        {
            return null;
        }

        var weatherInfo = new WeatherAlertInfo
        {
            // Temperature (°C)
            CurrentTemp = current.Temp,
            MinTemp = today?.Temp?.Min ?? current.Temp,
            MaxTemp = today?.Temp?.Max ?? current.Temp,
            
            // Humidity (%)
            Humidity = current.Humidity,
            
            // Precipitation
            PrecipitationProbability = (today?.Pop ?? 0) * 100, // Convert to percentage (%)
            PrecipitationAmount = today?.Rain ?? current.Rain?.OneHour ?? 0, // in mm
            
            // Wind (km/h and degrees)
            WindSpeed = current.WindSpeed * 3.6m, // Convert m/s to km/h
            WindGust = (current.WindGust > 0 ? current.WindGust : current.WindSpeed) * 3.6m, // Convert m/s to km/h
            WindDirection = current.WindDeg,
            
            // Atmospheric Pressure (hPa)
            Pressure = current.Pressure,
            
            // Solar Radiation (wh/m²)
            // UV Index (UVI) can be converted to approximate solar radiation
            // UVI * 25 ≈ solar radiation in W/m² (approximate)
            // For wh/m², we use the UVI value directly as a proxy
            SolarRadiation = current.Uvi * 25, // Approximate conversion
            
            // Sun Times (UTC)
            Sunrise = current.Sunrise > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(current.Sunrise).DateTime 
                : null,
            Sunset = current.Sunset > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(current.Sunset).DateTime 
                : null,
            
            // Moon Times (UTC) - From daily forecast
            Moonrise = today?.Moonrise > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(today.Moonrise).DateTime 
                : (DateTime?)null,
            Moonset = today?.Moonset > 0 
                ? DateTimeOffset.FromUnixTimeSeconds(today.Moonset).DateTime 
                : (DateTime?)null,
            
            // Location
            Latitude = latitude,
            Longitude = longitude,
            
            // Elevation (meters) - From elevation API
            Elevation = elevation,
            
            // Weather Description
            Description = current.Weather?.FirstOrDefault()?.Description ?? "N/A",
            
            // Forecast/Report Date
            ForecastDate = DateTime.UtcNow
        };

        return weatherInfo;
    }

    private async Task<decimal> GetElevationAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var response = await _elevationHttpClient.GetFromJsonAsync<ElevationApiResponse>(
                $"/lookup?locations={latitude},{longitude}");

            if (response?.Results != null && response.Results.Any())
            {
                var elevation = response.Results.First().Elevation;
                _logger?.LogInformation("Elevation for ({lat}, {lon}): {elevation}m", latitude, longitude, elevation);
                return elevation;
            }

            _logger?.LogWarning("No elevation data returned for ({lat}, {lon})", latitude, longitude);
            return 0;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching elevation data for ({lat}, {lon})", latitude, longitude);
            return 0; // Return 0 if elevation API fails
        }
    }
        public (decimal latitude, decimal longitude) GetCoordinatesForProject(Project project)
        {
            // Get weather coordinates based on project location
            // All KSA projects → Rabigh coordinates
            // All UAE projects → Expo 2020 Dubai coordinates

            return project.Location switch
            {
                ProjectLocationEnum.KSA => (22.80m, 39.03m), // Rabigh, Saudi Arabia
                ProjectLocationEnum.UAE => (25.0657m, 55.1713m), // Expo 2020 Dubai, UAE
                _ => (22.80m, 39.04m) // Default to Rabigh
            };
        }

        public string BuildWeatherAlertMessage(WeatherAlertInfo weatherInfo)
        {
            return $"Temperature: {weatherInfo.MinTemp:F1}-{weatherInfo.MaxTemp:F1}°C, " +
                   $"Precipitation: {weatherInfo.PrecipitationProbability:F0}% / {weatherInfo.PrecipitationAmount:F1}mm, " +
                   $"Wind: {weatherInfo.WindGust:F0}km/h, " +
                   $"Conditions: {weatherInfo.Description}.";
        }
    }
}
