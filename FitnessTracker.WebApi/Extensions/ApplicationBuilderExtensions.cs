using FitnessTracker.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace FitnessTracker.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFitnessTrackerPipeline(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseGlobalExceptionHandling();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
