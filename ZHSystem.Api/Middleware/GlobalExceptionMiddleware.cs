using System.Net;
using System.Text.Json;
using ZHSystem.Application.Common.Exceptions; 
using ZHSystem.Application.Common.Models;
namespace ZHSystem.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); 
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";
            IDictionary<string, string[]>? validationErrors = null;

           
            if (ex is AppException appEx)
            {
                statusCode = appEx.StatusCode; 
                message = appEx.Message;
                if (ex is ValidationException valex)
                {
                    validationErrors = valex.Errors;
                }
            }

            context.Response.StatusCode = statusCode;

           
            var response = new ApiErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Errors = validationErrors,
                
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
