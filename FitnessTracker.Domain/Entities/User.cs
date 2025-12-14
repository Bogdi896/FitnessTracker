using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(256)]
    public string Email { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [StringLength(10)]
    public string Gender { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Height { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Weight { get; set; }

    public DateTime RegistrationDate { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();

    [InverseProperty("User")]
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    [InverseProperty("User")]
    public virtual ICollection<MeasurementLog> MeasurementLogs { get; set; } = new List<MeasurementLog>();

    [InverseProperty("User")]
    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
