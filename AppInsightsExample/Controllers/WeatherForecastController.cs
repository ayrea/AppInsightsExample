using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace AppInsightsExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, TelemetryClient telemetryClient, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Get correlationId from inbound request header if it is available
            var correlationId = _httpContextAccessor.HttpContext?.Items["X-Correlation-ID"]?.ToString();

            var properties = new Dictionary<string, string>
            {
                { "CorrelationId", correlationId ?? "not-set" },
                { "CustomProperty", "some-value" }
            };

            _telemetryClient.TrackEvent("WeatherForecast_GetCalled", properties);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
