using System.Net;
using AIChat.Exceptions;

namespace AIChat.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(AIProviderException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode switch
                {
                    HttpStatusCode.BadRequest => StatusCodes.Status400BadRequest,
                    HttpStatusCode.Unauthorized => StatusCodes.Status502BadGateway,
                    HttpStatusCode.TooManyRequests => StatusCodes.Status429TooManyRequests,
                    HttpStatusCode.InternalServerError => StatusCodes.Status502BadGateway,
                    _ => StatusCodes.Status502BadGateway
                };
                await context.Response.WriteAsJsonAsync(new
                {
                    error = GetErrorMessage(ex.StatusCode)
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
                _ => "Error occured while communicating with AI Provider"
            };
        }
    }
}
