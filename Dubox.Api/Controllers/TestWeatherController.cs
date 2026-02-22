using Dubox.Domain.Services;
using Dubox.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dubox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestWeatherController : ControllerBase
    {
      private readonly IWeatherService _weatherService;
        private readonly WeatherMonitoringWorker _worker;
        public TestWeatherController( IWeatherService weatherService, WeatherMonitoringWorker worker )
        {
            _weatherService = weatherService;
            _worker = worker;
        }
        [HttpGet("check-weather")]
        public async Task<IActionResult> TestWeather(string city)
        {
            var result = await _weatherService.GetWeatherForecastAsync(city);
            return Ok(result);
        }
        [HttpPost("weather-run")]
        public async Task<IActionResult> RunWeatherNow()
        {
            await _worker.RunNowAsync();
            return Ok("Weather check executed immediately.");
        }
    }
}
