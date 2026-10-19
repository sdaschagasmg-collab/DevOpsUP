using System.Diagnostics;
using System.Text.Json;

namespace ProductsApi.Middleware;

// Mide la duración de cada request, que después sirve para la latencia (p50/p95).
// El level depende del status: 2xx/3xx Information, 4xx Warning, 5xx Error.
// Si hay una excepción no controlada, la registra como 500.

public class JsonRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public JsonRequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Exception? error = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            error = ex;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        finally
        {
            stopwatch.Stop();
            WriteLog(context, stopwatch.Elapsed.TotalMilliseconds, error);
        }
    }

    private static void WriteLog(HttpContext context, double durationMs, Exception? error)
    {
        var statusCode = context.Response.StatusCode;

        var level = statusCode >= 500 ? "Error"
                  : statusCode >= 400 ? "Warning"
                  : "Information";

        var entry = new Dictionary<string, object?>
        {
            ["timestamp"] = DateTime.UtcNow.ToString("O"),
            ["level"] = level,
            ["method"] = context.Request.Method,
            ["path"] = context.Request.Path.Value,
            ["status_code"] = statusCode,
            ["duration_ms"] = Math.Round(durationMs, 2),
            ["message"] = "HTTP request completed"
        };

        if (error is not null)
        {
            entry["error"] = error.Message;
        }

        Console.Out.WriteLine(JsonSerializer.Serialize(entry));
    }
}