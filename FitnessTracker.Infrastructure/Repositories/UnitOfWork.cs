using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Data;

namespace FitnessTracker.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FitnessTrackerDbContext _context;

        public IRepository<User> Users { get; }
        public IRepository<Workout> Workouts { get; }
        public IRepository<Exercise> Exercises { get; }
        public IRepository<Goal> Goals { get; }
        public IRepository<FoodItem> FoodItems { get; }
        public IRepository<FoodLog> FoodLogs { get; }
        public IRepository<MeasurementLog> MeasurementLogs { get; }
        public IRepository<WorkoutExercise> WorkoutExercises { get; }

        public UnitOfWork(FitnessTrackerDbContext context)
        {
            _context = context;

            Users = new Repository<User>(context);
            Workouts = new Repository<Workout>(context);
            Exercises = new Repository<Exercise>(context);
            Goals = new Repository<Goal>(context);
            FoodItems = new Repository<FoodItem>(context);
            FoodLogs = new Repository<FoodLog>(context);
            MeasurementLogs = new Repository<MeasurementLog>(context);
            WorkoutExercises = new Repository<WorkoutExercise>(context);
        }

        public Task<int> SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
