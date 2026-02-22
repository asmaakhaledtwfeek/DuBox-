using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Dubox.Application.DTOs
{
    public class ForecastRoot
    {
        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; }
    }

    public class ForecastItem
    {
        [JsonPropertyName("main")]
        public MainStats Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonPropertyName("dt_txt")]
        public string DateText { get; set; }
        
        [JsonPropertyName("wind")]
        public WindStats Wind { get; set; }
        
        [JsonPropertyName("pop")]
        public decimal Pop { get; set; } // Probability of precipitation (0-1)
        
        [JsonPropertyName("rain")]
        public PrecipitationStats Rain { get; set; }
    }

    public class CurrentWeatherRoot
    {
        [JsonPropertyName("coord")]
        public CoordinateStats Coord { get; set; }

        [JsonPropertyName("main")]
        public MainStats Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonPropertyName("wind")]
        public WindStats Wind { get; set; }

        [JsonPropertyName("sys")]
        public SysStats Sys { get; set; }

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class MainStats
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }

        [JsonPropertyName("temp_min")]
        public decimal TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public decimal TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public decimal Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
    
    public class WindStats
    {
        [JsonPropertyName("speed")]
        public decimal Speed { get; set; }
        
        [JsonPropertyName("gust")]
        public decimal Gust { get; set; }

        [JsonPropertyName("deg")]
        public decimal Deg { get; set; }
    }
    
    public class PrecipitationStats
    {
        [JsonPropertyName("3h")]
        public decimal ThreeHour { get; set; }
    }

    public class CoordinateStats
    {
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }

        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }
    }

    public class SysStats
    {
        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }

    // One Call API 3.0 Response Models
    public class OneCallApiResponse
    {
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }

        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("current")]
        public CurrentWeatherData Current { get; set; }

        [JsonPropertyName("daily")]
        public List<DailyWeatherData> Daily { get; set; }
    }

    public class CurrentWeatherData
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }

        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public decimal FeelsLike { get; set; }

        [JsonPropertyName("pressure")]
        public decimal Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }

        [JsonPropertyName("dew_point")]
        public decimal DewPoint { get; set; }

        [JsonPropertyName("uvi")]
        public decimal Uvi { get; set; }

        [JsonPropertyName("clouds")]
        public decimal Clouds { get; set; }

        [JsonPropertyName("visibility")]
        public decimal Visibility { get; set; }

        [JsonPropertyName("wind_speed")]
        public decimal WindSpeed { get; set; }

        [JsonPropertyName("wind_deg")]
        public decimal WindDeg { get; set; }

        [JsonPropertyName("wind_gust")]
        public decimal WindGust { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonPropertyName("rain")]
        public HourlyPrecipitation Rain { get; set; }
    }

    public class DailyWeatherData
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }

        [JsonPropertyName("moonrise")]
        public long Moonrise { get; set; }

        [JsonPropertyName("moonset")]
        public long Moonset { get; set; }

        [JsonPropertyName("moon_phase")]
        public decimal MoonPhase { get; set; }

        [JsonPropertyName("temp")]
        public TempStats Temp { get; set; }

        [JsonPropertyName("pressure")]
        public decimal Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }

        [JsonPropertyName("wind_speed")]
        public decimal WindSpeed { get; set; }

        [JsonPropertyName("wind_gust")]
        public decimal WindGust { get; set; }

        [JsonPropertyName("wind_deg")]
        public decimal WindDeg { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonPropertyName("pop")]
        public decimal Pop { get; set; }

        [JsonPropertyName("rain")]
        public decimal Rain { get; set; }

        [JsonPropertyName("uvi")]
        public decimal Uvi { get; set; }
    }

    public class TempStats
    {
        [JsonPropertyName("day")]
        public decimal Day { get; set; }

        [JsonPropertyName("min")]
        public decimal Min { get; set; }

        [JsonPropertyName("max")]
        public decimal Max { get; set; }

        [JsonPropertyName("night")]
        public decimal Night { get; set; }

        [JsonPropertyName("eve")]
        public decimal Eve { get; set; }

        [JsonPropertyName("morn")]
        public decimal Morn { get; set; }
    }

    public class HourlyPrecipitation
    {
        [JsonPropertyName("1h")]
        public decimal OneHour { get; set; }
    }

    // Open-Elevation API Response
    public class ElevationApiResponse
    {
        [JsonPropertyName("results")]
        public List<ElevationResult> Results { get; set; }
    }

    public class ElevationResult
    {
        [JsonPropertyName("latitude")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("elevation")]
        public decimal Elevation { get; set; }
    }
}
