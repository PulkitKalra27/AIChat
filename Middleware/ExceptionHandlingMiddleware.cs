using System.Net;
using AIChat.Exceptions;

namespace AIChat.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(AIProviderException ex)
            {
                _logger.LogError(ex, "Request failed with status code {StatusCode}", ex.StatusCode);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode switch
                {
                    HttpStatusCode.BadRequest => StatusCodes.Status400BadRequest,
                    HttpStatusCode.Unauthorized => StatusCodes.Status502BadGateway,
                    HttpStatusCode.TooManyRequests => StatusCodes.Status429TooManyRequests,
                    HttpStatusCode.InternalServerError => StatusCodes.Status502BadGateway,
                    HttpStatusCode.ServiceUnavailable => StatusCodes.Status503ServiceUnavailable,
                    _ => StatusCodes.Status502BadGateway
                };
                await context.Response.WriteAsJsonAsync(new
                {
                    error = GetErrorMessage(ex.StatusCode)
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Unexpected error occured while requesting.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "An unexpected error occured."
                });
            }
        }
        private static string GetErrorMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "AI Provider rejected the request.",
                HttpStatusCode.Unauthorized => "AI Provider Authentication failed.",
                HttpStatusCode.TooManyRequests => "AI Provider rate limit exceeded.",
                HttpStatusCode.InternalServerError => "AI Provider is currently unavailable.",
                HttpStatusCode.ServiceUnavailable => "AI Provider is currently unavailable.",
                _ => "Error occured while communicating with AI Provider"
            };
        }
    }
}
