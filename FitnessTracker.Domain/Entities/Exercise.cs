using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("Exercise")]
public class Exercise
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string MuscleGroup { get; set; } = null!;

    [StringLength(20)]
    public string DifficultyLevel { get; set; } = null!;

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
