using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Entities
{
    public class WeatherAlertInfo
    {
        // Temperature
        public decimal CurrentTemp { get; set; } // Current temperature in °C
        public decimal MinTemp { get; set; }
        public decimal MaxTemp { get; set; }
        
        // Humidity
        public decimal Humidity { get; set; } // in percentage
        
        // Precipitation
        public decimal PrecipitationProbability { get; set; } // in percentage
        public decimal PrecipitationAmount { get; set; } // in mm
        
        // Wind
        public decimal WindSpeed { get; set; } // in km/h
        public decimal WindGust { get; set; } // in km/h
        public decimal WindDirection { get; set; } // in degrees (0-360)
        
        // Pressure
        public decimal Pressure { get; set; } // in hPa
        
        // Solar Radiation
        public decimal SolarRadiation { get; set; } // in wh/m²
        
        // Sun and Moon
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunset { get; set; }
        public DateTime? Moonrise { get; set; }
        public DateTime? Moonset { get; set; }
        
        // Location
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; } // in meters
        
        // Description
        public string Description { get; set; }
        public DateTime ForecastDate { get; set; }
        
        // Method to check if weather is favorable for construction
        public bool IsFavorableForConstruction()
        {
            // Weather is unfavorable if:
            // - Temperature outside 19-26°C range
            // - Precipitation probability > 0%
            // - Precipitation amount > 0mm
            // - Wind gust > 24 km/h
            // - Description indicates poor conditions
            
            //bool tempOutOfRange = MinTemp < 19 || MaxTemp > 26;
            //bool hasPrecipitation = PrecipitationProbability > 0 || PrecipitationAmount > 0;
            bool highWind = WindGust > 24;
            
            return !( highWind);
        }
    }
}
