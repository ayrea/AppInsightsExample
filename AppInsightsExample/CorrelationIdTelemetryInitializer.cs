using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsExample
{
    public class CorrelationIdTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            if (context.Items.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                telemetry.Context.Properties["CorrelationId"] = correlationId.ToString();
            }
        }
    }
}
