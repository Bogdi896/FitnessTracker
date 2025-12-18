using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex,
            ILogger logger)
        {
            var traceId = context.TraceIdentifier;

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "An unexpected error occurred.";
            var detail = ex.Message;

            switch (ex)
            {
                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    title = "Validation error";
                    break;

                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.Conflict; // 409
                    title = "Business rule violation";
                    break;

                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    title = "Resource not found";
                    break;

            }

            logger.LogError(ex,
                "Unhandled exception. StatusCode: {StatusCode}, TraceId: {TraceId}",
                statusCode, traceId);

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = traceId;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);
        }
    }
}
