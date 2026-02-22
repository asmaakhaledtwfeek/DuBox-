using Dubox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Domain.Services
{
    public interface IWeatherService
    {
        Task<WeatherAlertInfo> GetWeatherForecastAsync(string city);
        Task<WeatherAlertInfo> GetWeatherForecastByCoordinatesAsync(decimal latitude, decimal longitude);
        string BuildWeatherAlertMessage(WeatherAlertInfo weatherInfo);
        public (decimal latitude, decimal longitude) GetCoordinatesForProject(Project project);
    }
}
