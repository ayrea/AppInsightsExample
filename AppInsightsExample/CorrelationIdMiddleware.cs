namespace AppInsightsExample
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers.ContainsKey(CorrelationIdHeaderName)
                ? context.Request.Headers[CorrelationIdHeaderName].ToString()
                : Guid.NewGuid().ToString();

            // Store it in HttpContext.Items for access later
            context.Items[CorrelationIdHeaderName] = correlationId;

            // Add to response headers for downstream visibility
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeaderName] = correlationId;
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
