using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Domain.Entities;
using Serilog;
using System.Diagnostics;
using System.Security.Claims;
namespace ZHSystem.Api.Middleware
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public AuditLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<AuditLoggingMiddleware>();
        }

     
        public async Task InvokeAsync(HttpContext context ,IAuditService auditService)
        {

            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (path.Contains("swagger") || path.Contains("health")))
            {
                await _next(context); 
                return;
            }
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var email = context.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Anonymous";
            using (Serilog.Context.LogContext.PushProperty("UserId", userId))
            using (Serilog.Context.LogContext.PushProperty("UserEmail", email))
            using (Serilog.Context.LogContext.PushProperty("Path", context.Request.Path))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();



                    try
                    {
                        var method = context.Request.Method.ToUpper();
                        if (method == "POST" || method == "PUT" || method == "DELETE")
                        {
                            var auditLog = new AuditLog
                            {
                                UserId = userId,
                                UserEmail = email,
                                Method = context.Request.Method,
                                Path = context.Request.Path,
                                StatusCode = context.Response.StatusCode,
                                DurationMs = stopwatch.ElapsedMilliseconds,
                                IpAddress = context.Connection.RemoteIpAddress?.ToString()
                            };
                            await auditService.WriteAsync(auditLog);
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to save AuditLog: {ex.Message}");
                    }
                   
                    {
                        _logger.Information(
                            "HTTP {Method} {Path} responded {Status} in {Duration}ms | IP: {IP}",
                            context.Request.Method,
                            context.Request.Path,
                            context.Response.StatusCode,
                            stopwatch.ElapsedMilliseconds,
                            context.Connection.RemoteIpAddress?.ToString()
                        );
                    }
                }
            }
        }
                

            

    }
}
