using Serilog.Context;

namespace CurrencyRates.GatewayService.Api.Middleware;

/// <summary>
/// Генерирует или принимает <c>X-Correlation-Id</c> до проксирования в downstream-сервисы.
/// </summary>
public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    /// <summary>Имя HTTP-заголовка correlation id.</summary>
    public const string HeaderName = "X-Correlation-Id";

    /// <summary>
    /// Читает или генерирует correlation id, пробрасывает в request (для YARP) и LogContext.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
            context.Request.Headers[HeaderName] = correlationId;
        }

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }
}
