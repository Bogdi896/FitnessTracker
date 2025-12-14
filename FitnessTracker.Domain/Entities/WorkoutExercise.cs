using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("WorkoutExercise")]
public class WorkoutExercise
{
    public int WorkoutId { get; set; }

    public int ExerciseId { get; set; }

    public int Sets { get; set; }

    public int Reps { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? WeightUsed { get; set; }

    [ForeignKey("ExerciseId")]
    [InverseProperty("WorkoutExercises")]
    public virtual Exercise Exercise { get; set; } = null!;

    [ForeignKey("WorkoutId")]
    [InverseProperty("WorkoutExercises")]
    public virtual Workout Workout { get; set; } = null!;
}
