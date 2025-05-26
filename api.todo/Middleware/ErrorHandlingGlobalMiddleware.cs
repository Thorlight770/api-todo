public class ErrorHandlingGlobalMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingGlobalMiddleware> _logger;

    public ErrorHandlingGlobalMiddleware(RequestDelegate next, ILogger<ErrorHandlingGlobalMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal Server Error !");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new { message = "An error occurred.", detail = ex.Message };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
