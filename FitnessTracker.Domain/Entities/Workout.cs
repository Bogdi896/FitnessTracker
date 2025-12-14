using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("Workout")]
public partial class Workout
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int DurationInMinutes { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Workouts")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Workout")]
    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
