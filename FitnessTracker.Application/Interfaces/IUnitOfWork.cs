using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Workout> Workouts { get; }
        IRepository<Exercise> Exercises { get; }
        IRepository<WorkoutExercise> WorkoutExercises { get; }
        IRepository<FoodItem> FoodItems { get; }
        IRepository<FoodLog> FoodLogs { get; }
        IRepository<Goal> Goals { get; }
        IRepository<MeasurementLog> MeasurementLogs { get; }
        Task<int> SaveChangesAsync();

    }
}
