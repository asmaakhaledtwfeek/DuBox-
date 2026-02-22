using System;

namespace Dubox.Application.DTOs
{
    public class ProjectWeatherReportDto
    {
        public Guid ReportId { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        
        // Temperature
        public decimal CurrentTemperature { get; set; }
        public decimal MinTemperature { get; set; }
        public decimal MaxTemperature { get; set; }
        
        // Humidity
        public decimal Humidity { get; set; }
        
        // Precipitation
        public decimal PrecipitationProbability { get; set; }
        public decimal PrecipitationAmount { get; set; }
        
        // Wind
        public decimal WindSpeed { get; set; }
        public decimal WindGust { get; set; }
        public decimal WindDirection { get; set; }
        
        // Pressure
        public decimal Pressure { get; set; }
        
        // Solar Radiation
        public decimal SolarRadiation { get; set; }
        
        // Sun and Moon
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunset { get; set; }
        public DateTime? Moonrise { get; set; }
        public DateTime? Moonset { get; set; }
        
        // Location
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }
        
        public string Description { get; set; } = string.Empty;
        public bool IsFavorable { get; set; }
        public string? AlertMessage { get; set; }
        public bool QualityIssueCreated { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
