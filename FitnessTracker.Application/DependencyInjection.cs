using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Services;

namespace FitnessTracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();
            services.AddScoped<IGoalService, GoalService>();
            services.AddScoped<IFoodItemService, FoodItemService>();
            services.AddScoped<IFoodLogService, FoodLogService>();
            services.AddScoped<IMeasurementLogService, MeasurementLogService>();

            return services;
        }
    }
}
