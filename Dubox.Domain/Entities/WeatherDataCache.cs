using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubox.Domain.Entities
{
    [Table("WeatherDataCaches")]
    public class WeatherDataCache
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CacheId { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Temperature
        [Column(TypeName = "decimal(5,2)")]
        public decimal CurrentTemperature { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal MinTemperature { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal MaxTemperature { get; set; }

        // Humidity
        [Column(TypeName = "decimal(5,2)")]
        public decimal Humidity { get; set; } // Percentage

        // Precipitation
        [Column(TypeName = "decimal(5,2)")]
        public decimal PrecipitationProbability { get; set; } // Percentage

        [Column(TypeName = "decimal(6,2)")]
        public decimal PrecipitationAmount { get; set; } // mm

        // Wind
        [Column(TypeName = "decimal(6,2)")]
        public decimal WindSpeed { get; set; } // km/h

        [Column(TypeName = "decimal(6,2)")]
        public decimal WindGust { get; set; } // km/h

        [MaxLength(50)]
        public decimal WindDirection { get; set; } 

        // Pressure
        [Column(TypeName = "decimal(7,2)")]
        public decimal Pressure { get; set; } // hPa

        // Solar Radiation
        [Column(TypeName = "decimal(8,2)")]
        public decimal SolarRadiation { get; set; } // wh/m²

        // Sun and Moon
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunset { get; set; }
        public DateTime? Moonrise { get; set; }
        public DateTime? Moonset { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,7)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,7)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal Elevation { get; set; } // meters

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
