using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Data;

public partial class FitnessTrackerDbContext : DbContext
{
    public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<FoodItem> FoodItems => Set<FoodItem>();
    public DbSet<FoodLog> FoodLogs => Set<FoodLog>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<MeasurementLog> MeasurementLogs => Set<MeasurementLog>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Exercise
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // FoodItem
        modelBuilder.Entity<FoodItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Name)
                  .IsUnique()
                  .HasDatabaseName("UQ__FoodItem__737584F6DBDCB487");
        });

        // FoodLog
        modelBuilder.Entity<FoodLog>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Food).WithMany(p => p.FoodLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FoodLog_FoodItem");

            entity.HasOne(d => d.User).WithMany(p => p.FoodLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FoodLog_User");
        });

        // Goal
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.User).WithMany(p => p.Goals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_User");
        });

        // MeasurementLog
        modelBuilder.Entity<MeasurementLog>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.User).WithMany(p => p.MeasurementLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeasurementLog_User");
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RegistrationDate)
                  .HasDefaultValueSql("(sysutcdatetime())");
        });

        // Workout
        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.User).WithMany(p => p.Workouts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workout_User");
        });

        // WorkoutExercise (composite key)
        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => new { e.WorkoutId, e.ExerciseId });

            entity.Property(e => e.WeightUsed).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutExercises)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutExercise_Exercise");

            entity.HasOne(d => d.Workout).WithMany(p => p.WorkoutExercises)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutExercise_Workout");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
