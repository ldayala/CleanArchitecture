using Serilog.Context;

namespace CleanArchitecture.Api.Middleware
{
    public class RequestContextLoggingMiddlewaer
    {
        //lo voya obtener del request del cliente
        private const string CorrelationIdHeaderName = "X-Correlation-ID";

        private readonly RequestDelegate _next;
        public RequestContextLoggingMiddlewaer(RequestDelegate next)
        {
            _next = next;
        }
            
        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the correlation ID header is present
            using (LogContext.PushProperty("CorrelationId", GetCorrelationID(context))) {
              await  _next(context);
            }
            
        }

        private static string GetCorrelationID(HttpContext context)
        {

            context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
           
        }
    }
}
