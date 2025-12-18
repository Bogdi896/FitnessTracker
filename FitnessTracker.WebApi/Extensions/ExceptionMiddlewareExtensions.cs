using FitnessTracker.WebApi.Middleware;

namespace FitnessTracker.WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
